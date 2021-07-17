using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialScenario : MonoBehaviour
{
    public GameObject secondTitle;
    public Image handCursor;
    public Image patternToDrawHorizontal;
    public Image patternToDrawVertical;

    [SerializeField] private Vector2 drawingHorizontalStartPoint;
    [SerializeField] private Vector2 drawingHorizontalEndPoint;
    [SerializeField] private Vector2 drawingVerticalStartPoint;
    [SerializeField] private Vector2 drawingVerticalEndPoint;
    [SerializeField] private Vector2 startPoint;
    [SerializeField] private Vector2 finishPoint;

    public bool isHorizontalDraw = false;
    public bool isVerticalDraw = false;

    private Sequence horizontalDraw;
    private Sequence verticalDraw;

    // Start is called before the first frame update
    void Start()
    {
        handCursor.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (!isHorizontalDraw)
        {
            horizontalDraw = DOTween.Sequence();

            horizontalDraw
                .Append(handCursor.rectTransform.DOAnchorPos(startPoint, 0.01f))
                .AppendCallback(() => handCursor.enabled = true)
                .Append(handCursor.rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.5f))
                .Join(handCursor.rectTransform.DOAnchorPos(drawingHorizontalStartPoint, 0.5f))
                .AppendCallback(() => {
                    patternToDrawHorizontal.rectTransform.localScale = new Vector2(0f, 1f);
                })
                .Append(handCursor.rectTransform.DOAnchorPos(drawingHorizontalEndPoint, 0.5f))
                .Join(patternToDrawHorizontal.rectTransform.DOScaleX(1f, 0.5f))
                .Append(handCursor.rectTransform.DOAnchorPos(finishPoint, 0.5f))
                .Join(handCursor.rectTransform.DORotate(new Vector3(0f, 0f, -90f), 0.5f))
                .Join(patternToDrawHorizontal.DOFade(0f, 0.5f))
                .SetLoops(-1);

            horizontalDraw.Play();

            isHorizontalDraw = true;
            LevelManager.isLevelStarted = true;
        }
        else if (isHorizontalDraw && !isVerticalDraw)
        {
            secondTitle.SetActive(true);

            verticalDraw = DOTween.Sequence();

            verticalDraw
                .Append(handCursor.rectTransform.DOAnchorPos(startPoint, 0.01f))
                .AppendCallback(() => handCursor.enabled = true)
                .Append(handCursor.rectTransform.DORotate(new Vector3(0f, 0f, -90f), 0.5f))
                .Join(handCursor.rectTransform.DOAnchorPos(drawingVerticalStartPoint, 0.5f))
                .AppendCallback(() =>
                {
                    patternToDrawVertical.rectTransform.localScale = new Vector2(1f, 0f);
                })
                .Append(handCursor.rectTransform.DOAnchorPos(drawingVerticalEndPoint, 1f))
                .Join(patternToDrawVertical.rectTransform.DOScaleY(1f, 1f))
                .Append(handCursor.rectTransform.DOAnchorPos(drawingHorizontalStartPoint, 0.5f))
                .Join(handCursor.rectTransform.DORotate(new Vector3(0f, 0f, 0f), 0.5f))
                .Join(patternToDrawVertical.DOFade(0f, 0.5f))
                .AppendCallback(() => handCursor.enabled = true)
                .AppendCallback(() => {
                    patternToDrawHorizontal.rectTransform.localScale = new Vector2(0f, 1f);
                })
                .Append(handCursor.rectTransform.DOAnchorPos(drawingHorizontalEndPoint, 1f))
                .Join(patternToDrawHorizontal.rectTransform.DOScaleX(1f, 1f))
                .Append(handCursor.rectTransform.DOAnchorPos(finishPoint, 0.5f))
                .Join(handCursor.rectTransform.DORotate(new Vector3(0f, 0f, -90f), 0.5f))
                .Join(patternToDrawHorizontal.DOFade(0f, 0.5f))
                .SetLoops(-1);

            verticalDraw.Play();

            isVerticalDraw = true;
        }
    }

    private void OnDisable()
    {
        if (horizontalDraw.IsPlaying() && isHorizontalDraw)
        {
            horizontalDraw.Complete();
            horizontalDraw.Kill();
            patternToDrawHorizontal.DOFade(1f, 0f);
            patternToDrawHorizontal.rectTransform.DOScaleX(0f, 0f);
            handCursor.enabled = false;
        }

        else if (verticalDraw.IsPlaying() && isVerticalDraw)
        {
            verticalDraw.Complete();
            verticalDraw.Kill();
            patternToDrawVertical.DOFade(1f, 0f);
            patternToDrawVertical.rectTransform.DOScaleY(1f, 0f);
            handCursor.enabled = false;
        }
    }

    public void HideTutorialPanel()
    {
        gameObject.SetActive(false);
    }

    public void ShowTutorialPanel()
    {
        gameObject.SetActive(true);
    }
}
