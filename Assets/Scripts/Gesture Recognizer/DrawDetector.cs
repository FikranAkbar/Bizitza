using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using FMODUnity;

namespace GestureRecognizer {

	/// <summary>
	/// Captures player drawing and call the Recognizer to discover which gesture player id.
	/// Calls 'OnRecognize' event when something is recognized.
	/// </summary>
	public class DrawDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		private Canvas trainingCanvas;
		private TutorialScenario tutorialScenarioController;

		public ParticleSystem drawEffect;
		public ParticleSystem playerStickEffect;

		public Recognizer recognizer;
		public UILineRenderer line;
		private List<UILineRenderer> lines;

		[Range(0f,1f)]
		public float scoreToAccept = 0.8f;

		[Range(1,10)]
		public int minLines = 1;
		public int MinLines { set { minLines = Mathf.Clamp (value, 1, 10); } }

		[Range(1,10)]
		public int maxLines = 2;
		public int MaxLines { set { maxLines = Mathf.Clamp (value, 1, 10); } }

		public enum RemoveStrategy { RemoveOld, ClearAll }
		public RemoveStrategy removeStrategy;

		public bool clearNotRecognizedLines;

		public bool fixedArea = false;

		GestureData data = new GestureData();

		[System.Serializable]
		public class ResultEvent : UnityEvent<RecognitionResult> {}
		public ResultEvent SendIDToPlayerGesture;
		public ResultEvent OnRecognize;

		public GameObject startMagicSwapSound;
		public GameObject gestureCorrectSound;
		Coroutine gestureCorrectCoroutine;

		RectTransform rectTransform;

		void Start() {
			if (GameObject.FindGameObjectWithTag("Training Canvas") != null)
            {
				trainingCanvas = GameObject.FindGameObjectWithTag("Training Canvas").GetComponent<Canvas>();
				tutorialScenarioController = GameObject.FindGameObjectWithTag("Training Canvas").GetComponent<TutorialScenario>();
			}

			drawEffect.Stop();
			playerStickEffect.Stop();
			line.enabled = true;
			line.relativeSize = true;
			line.LineList = false;
			lines = new List<UILineRenderer> (){ line };
			rectTransform = transform as RectTransform;
			UpdateLines ();
		}

        void OnValidate(){
			maxLines = Mathf.Max (minLines, maxLines);
		}

		public void UpdateLines(){
			while (lines.Count < data.lines.Count) {
				var newLine = Instantiate (line, line.transform.parent);
				lines.Add (newLine);
			}
			for (int i = 0; i < lines.Count; i++) {
				lines [i].Points = new Vector2[]{ };
				lines [i].SetAllDirty ();
			}
			int n = Mathf.Min (lines.Count, data.lines.Count);
			for (int i = 0; i < n; i++) {
				lines [i].Points = data.lines [i].points.Select (p => RealToLine (p)).ToArray ();
				lines [i].SetAllDirty ();
			}
		}

		Vector2 RealToLine(Vector2 position){
			var local = rectTransform.InverseTransformPoint (position);
			var normalized = Rect.PointToNormalized (rectTransform.rect, local);
			return normalized;
		}

		Vector2 FixedPosition(Vector2 position){
			return position;
			//var local = rectTransform.InverseTransformPoint (position);
			//var normalized = Rect.PointToNormalized (rectTransform.rect, local);
			//return normalized;
		}

		public void ClearLines(){
			data.lines.Clear ();
			UpdateLines ();
		}
		
        public void OnBeginDrag (PointerEventData eventData) {

			if (LevelManager.isLevelStarted && !LevelManager.isLevelFailed)
            {
				startMagicSwapSound.SetActive(true);
				if (trainingCanvas != null)
				{
					trainingCanvas.enabled = false;
					tutorialScenarioController.enabled = false;
				}

				drawEffect.Play();
				playerStickEffect.Play();

				line.DOKill(true);
				line.color = new Color(1f, 1f, 1f);
				line.DOFade(1f, 0.001f);

				if (data.lines.Count >= maxLines)
				{
					switch (removeStrategy)
					{
						case RemoveStrategy.RemoveOld:
							data.lines.RemoveAt(0);
							break;
						case RemoveStrategy.ClearAll:
							data.lines.Clear();
							break;
					}
				}

				data.lines.Add(new GestureLine());

				var fixedPos = FixedPosition(eventData.position);
				if (data.LastLine.points.Count == 0 || data.LastLine.points.Last() != fixedPos)
				{
					data.LastLine.points.Add(fixedPos);
					UpdateLines();
				}
			}
		}

		public void OnDrag (PointerEventData eventData) {
			if (LevelManager.isLevelStarted && !LevelManager.isLevelFailed)
			{
				var fixedPos = FixedPosition(eventData.position);
				if (data.LastLine.points.Count == 0 || data.LastLine.points.Last() != fixedPos)
				{
					data.LastLine.points.Add(fixedPos);
					UpdateLines();
				}

				Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
				Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

				drawEffect.gameObject.transform.position = objPos;
			}
		}

		public void OnEndDrag (PointerEventData eventData)
		{
			if (LevelManager.isLevelStarted && !LevelManager.isLevelFailed)
			{
				startMagicSwapSound.SetActive(false);
				drawEffect.Stop();
				playerStickEffect.Stop();
				StartCoroutine(OnEndDragCoroutine(eventData, line));
			}
			
		}

		IEnumerator OnEndDragCoroutine(PointerEventData eventData, UILineRenderer line){

			data.LastLine.points.Add (FixedPosition(eventData.position));
			UpdateLines ();

			for (int size = data.lines.Count; size >= 1 && size >= minLines; size--) {
				//last [size] lines
				var sizedData = new GestureData () {
					lines = data.lines.GetRange (data.lines.Count - size, size)
				};

				var sizedNormalizedData = sizedData;

				if (fixedArea) {
					var rect = this.rectTransform.rect;
					sizedNormalizedData = new GestureData (){
						lines = sizedData.lines.Select( mLine => new GestureLine(){
							closedLine = mLine.closedLine,
							points = mLine.points.Select( p => Rect.PointToNormalized(rect, this.rectTransform.InverseTransformPoint(p) ) ).ToList()
						} ).ToList()
					};
				}

				RecognitionResult result = null;

				//run in another thread

				var thread = new System.Threading.Thread (()=>{
					result = recognizer.Recognize (sizedNormalizedData, normalizeScale: !fixedArea);
				});
				thread.Start ();
				while (thread.IsAlive) {
					yield return null;
				}

				if (result.gesture != null && result.score.score >= scoreToAccept) {

					OnRecognize.Invoke(result);
					SendIDToPlayerGesture.Invoke(result);
					if (gestureCorrectCoroutine == null)
                    {
						gestureCorrectCoroutine = StartCoroutine(PlayDrawPatternSuccessSound());
                    }
					line.color = LineColorValue.GetColor(result.gesture.id);
					line.DOFade(0f, 0.01f);
					if (clearNotRecognizedLines) {
						data = sizedData;
						UpdateLines ();
					}
					break;
				} else {
					OnRecognize.Invoke(RecognitionResult.Empty);
					SendIDToPlayerGesture.Invoke(RecognitionResult.Empty);
					line.DOFade(0f, 1f);
				}
			}

			yield return null;
		}


		private IEnumerator PlayDrawPatternSuccessSound()
        {
			gestureCorrectSound.SetActive(true);
			yield return new WaitUntil(gestureCorrectSound.GetComponent<StudioEventEmitter>().IsPlaying);
			gestureCorrectSound.SetActive(false);
			gestureCorrectCoroutine = null;
        }
	}
}