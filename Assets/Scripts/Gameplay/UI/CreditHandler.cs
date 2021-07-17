using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using TMPro;

public class CreditHandler : MonoBehaviour
{
    public RectTransform credit;
    public int endPoint;
    public int startPoint;
    public float duration;
    public Image blackScreen;
    public GameObject creditSong;
    public TextMeshProUGUI toBeContText;
    public TextMeshProUGUI thanksText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpaceToSkip());
        DOTween.Sequence()
            .AppendInterval(2f)
            .Append(toBeContText.DOFade(1f, 1f))
            .AppendInterval(2f)
            .Append(toBeContText.DOFade(0f, 1f))
            .AppendInterval(1f)
            .Append(thanksText.DOFade(1f, 1f))
            .AppendInterval(2f)
            .Append(thanksText.DOFade(0f, 1f))
            .Append(credit.DOAnchorPosY(endPoint, duration).SetEase(Ease.Unset))
            .AppendInterval(2f)
            .AppendCallback(() => 
            {
                SceneManager.LoadScene(2);
            });
    }

    IEnumerator SpaceToSkip()
    {
        yield return new WaitUntil(() => 
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKeyDown(KeyCode.Escape) || 
            Input.GetKeyDown(KeyCode.Return) || 
            Input.GetKeyDown(KeyCode.Backspace)
            );
        DOTween.Sequence()
            .Append(blackScreen.DOFade(1f, 1f))
            .AppendInterval(1f)
            .AppendCallback(() =>
           {
               creditSong.SetActive(false);
               SceneManager.LoadScene(2);
           });
        var studioEventEmitter = creditSong.GetComponent<StudioEventEmitter>();
        studioEventEmitter.EventInstance.setVolume(1f);
        for (float i = 1f; i > 0f; i -= 0.1f)
        {
            if (i < 0f)
                i = 0f;
            studioEventEmitter.EventInstance.setVolume(i);
            yield return new WaitForSeconds(0.1f);
        }
        creditSong.SetActive(false);
    }
}
