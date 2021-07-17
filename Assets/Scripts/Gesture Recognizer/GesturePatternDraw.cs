using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GestureRecognizer {

	/// <summary>
	/// Renderer to automatic draw gesture on Canvas
	/// </summary>
	public class GesturePatternDraw : UILineRenderer {

		public GesturePattern pattern;

		public float endingSize = 0;

        private void OnValidate()
        {
			this.relativeSize = true;
			this.endingSize = Mathf.Max (0, this.endingSize);
			this.LineList = true;
			SetAllDirty ();
		}

		protected override void OnPopulateMesh (UnityEngine.UI.VertexHelper vh) {
			DrawPattern ();
			base.OnPopulateMesh (vh);
		}

		public void DrawPattern() {

			if (pattern == null || pattern.gesture == null) {
				return;
			}

			var size = this.rectTransform.sizeDelta;

			Rect rect = this.rectTransform.rect;
			rect.center += rect.size / 2;

			var patternPoints = new List<Vector2> ();

			for (int i = 0; i < pattern.gesture.lines.Count; i++) {
				var points = pattern.gesture.lines[i].points;
				var intCloseLine = pattern.gesture.lines [i].closedLine ? 1 : 0;
				for (int j = 0; j < points.Count-1 + intCloseLine; j++) {
					//patternPoints.Add (Rect.NormalizedToPoint (rect, points [j]));
					//patternPoints.Add (Rect.NormalizedToPoint (rect, points [j + 1]));
					patternPoints.Add (points [j]);
					patternPoints.Add (points [(j + 1) % points.Count]);
				}
				if (endingSize > 0) {
					var c = Rect.NormalizedToPoint (rect, points.Last ());
					patternPoints.Add (c + new Vector2 (-1, -1) * endingSize);
					patternPoints.Add (c + new Vector2 (1, 1) * endingSize);
					patternPoints.Add (c + new Vector2 (-1, 1) * endingSize);
					patternPoints.Add (c + new Vector2 (1, -1) * endingSize);
				}
			}


			this.Points = patternPoints.ToArray ();
			this.CrossFadeColor(LineColorValue.GetColor(pattern.id), 0.01f, true, false);
		}
	}

}