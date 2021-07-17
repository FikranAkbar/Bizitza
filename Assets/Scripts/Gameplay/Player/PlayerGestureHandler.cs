using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerGestureHandler : MonoBehaviour
{
    public GestureRecognizer.DrawDetector drawDetector;
    public GameObject patternDrawerPrefab;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AddListenerToDrawDetector());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AddListenerToDrawDetector()
    {
        yield return new WaitUntil(() => drawDetector != null);
        drawDetector.SendIDToPlayerGesture.AddListener(AddPatternDrawingIfNotEmpty);
    }

    public void AddPatternDrawingIfNotEmpty(GestureRecognizer.RecognitionResult result)
    {
        if (transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (result != GestureRecognizer.RecognitionResult.Empty)
        {
            var playerGestureBackground = Instantiate(patternDrawerPrefab, transform);
            var gesturePatternDrawBackground = playerGestureBackground.GetComponent<GestureRecognizer.GesturePatternDraw>();
            gesturePatternDrawBackground.rectTransform.sizeDelta = new Vector2(25f, 25f);
            gesturePatternDrawBackground.pattern = result.gesture;
            gesturePatternDrawBackground.transform.DOScale(0f, 0.0001f);

            var playerGesture = Instantiate(patternDrawerPrefab, transform);
            var gesturePatternDraw = playerGesture.GetComponent<GestureRecognizer.GesturePatternDraw>();
            gesturePatternDrawBackground.LineThickness = gesturePatternDraw.LineThickness + 4;
            gesturePatternDraw.rectTransform.sizeDelta = new Vector2(25f, 25f);
            gesturePatternDraw.pattern = result.gesture;
            playerGesture.transform.DOScale(0f, 0.0001f);

            var gesturePattern = gesturePatternDraw.rectTransform;
            var gesturePatternBackground = gesturePatternDrawBackground.rectTransform;
            DOTween.Sequence()
                .AppendCallback(() => gesturePatternDrawBackground.CrossFadeColor(Color.white, 0.001f, true, false))
                .AppendCallback(() =>
                {
                    PlayPlayerDrawingAnim(result.gesture.id);
                })
                .Join(gesturePattern.DOScale(1.2f, 0.4f))
                .Join(gesturePatternBackground.DOScale(1.2f, 0.4f))
                .Append(gesturePattern.DOScale(0.8f, 0.2f))
                .Join(gesturePatternBackground.DOScale(0.8f, 0.2f))
                .Append(gesturePattern.DOScale(1f, 0.2f))
                .Join(gesturePatternBackground.DOScale(1f, 0.2f))
                .AppendInterval(0.4f)
                .Append(gesturePattern.DOScale(0f, 0.5f))
                .Join(gesturePatternBackground.DOScale(0f, 0.5f))
                .AppendCallback(() =>
                {
                    Destroy(playerGesture);
                    Destroy(playerGestureBackground);
                });
        }
    }

    public void PlayPlayerDrawingAnim(string id)
    {
        if (id == "horizontal")
        {
            animator.Play("Base Layer.Horizontal-Pattern_Player", 0, 0f);
        }
        else if (id == "vertical")
        {
            animator.Play("Base Layer.Vertical-Pattern_Player", 0, 0f);
        }
        else if (id == "up")
        {
            animator.Play("Base Layer.Up-Pattern_Player", 0, 0f);
        }
        else if (id == "down")
        {
            animator.Play("Base Layer.Down-Pattern_Player", 0, 0f);
        }
        else if (id == "right")
        {
            animator.Play("Base Layer.Right-Pattern_Player", 0, 0f);
        }
        else if (id == "left")
        {
            animator.Play("Base Layer.Left-Pattern_Player", 0, 0f);
        }
    }

    public void PlayPlayerDeadAnim()
    {
        animator.Play("Base Layer.Dead");
    }
}
