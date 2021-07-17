using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;

public class PrologManager : MonoBehaviour
{
    public Image unityLogo;
    public TextMeshProUGUI unityHeaderText;

    public Image fmodLogo;
    public TextMeshProUGUI fmodHeaderText;

    public Image doTweenLogo;
    public TextMeshProUGUI doTweenHeaderText;

    public List<PrologScenario> prologScenarios;
    public List<GameObject> prologComicBoxes;

    public Image backgroundProlog;
    public Image blackScreen;

    public float slideDuration;

    public GameObject prologPageSound;
    Coroutine prologPageCoroutine;
    public GameObject prologSound;
    Coroutine prologCoroutine;

    public bool isAllPageOut = false;

    // Start is called before the first frame update
    void Start()
    {
        prologCoroutine = StartCoroutine(PlayPrologSong());

        Sequence splashLogosSeqeuence = DOTween.Sequence();
        splashLogosSeqeuence

            .AppendInterval(3f)
            .Append(unityHeaderText.DOFade(1f, 1f))
            .Join(unityLogo.DOFade(1f, 1f))
            .AppendInterval(2f)
            .Append(unityHeaderText.DOFade(0f, 1f))
            .Join(unityLogo.DOFade(0f, 1f))

            .Append(fmodHeaderText.DOFade(1f, 1f))
            .Join(fmodLogo.DOFade(1f, 1f))
            .AppendInterval(2f)
            .Append(fmodHeaderText.DOFade(0f, 1f))
            .Join(fmodLogo.DOFade(0f, 1f))

            .Append(doTweenHeaderText.DOFade(1f, 1f))
            .Join(doTweenLogo.DOFade(1f, 1f))
            .AppendInterval(2f)
            .Append(doTweenHeaderText.DOFade(0f, 1f))
            .Join(doTweenLogo.DOFade(0f, 1f))

            .OnComplete(() =>
            {
                Sequence prologPagesSequence = DOTween.Sequence();
                InitPrologPagesSequence(prologPagesSequence);
            });
    }

    private void InitPrologPagesSequence(Sequence prologPagesSequence)
    {
        for (int i = 0; i < prologScenarios.Count; i++)
        {
            prologComicBoxes[i].GetComponent<Image>().sprite = prologScenarios[i].GetComicPicutre();
        }

        prologPagesSequence
            .AppendCallback(() =>
            {
                if (prologPageCoroutine == null)
                {
                    StartCoroutine(PlayPrologPageSound());
                }
            })
            .Join(prologComicBoxes[0].GetComponent<RectTransform>().DOAnchorPos(prologScenarios[0].GetEndPoint(), slideDuration))
            .Join(prologComicBoxes[0].GetComponent<Image>().DOFade(1f, slideDuration))
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                if (prologPageCoroutine == null)
                {
                    StartCoroutine(PlayPrologPageSound());
                }
            })
            .Append(prologComicBoxes[1].GetComponent<RectTransform>().DOAnchorPos(prologScenarios[1].GetEndPoint(), slideDuration))
            .Join(prologComicBoxes[1].GetComponent<Image>().DOFade(1f, slideDuration))
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                if (prologPageCoroutine == null)
                {
                    StartCoroutine(PlayPrologPageSound());
                }
            })
            .Append(prologComicBoxes[2].GetComponent<RectTransform>().DOAnchorPos(prologScenarios[2].GetEndPoint(), slideDuration))
            .Join(prologComicBoxes[2].GetComponent<Image>().DOFade(1f, slideDuration))
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                if (prologPageCoroutine == null)
                {
                    StartCoroutine(PlayPrologPageSound());
                }
            })
            .Append(prologComicBoxes[3].GetComponent<RectTransform>().DOAnchorPos(prologScenarios[3].GetEndPoint(), slideDuration))
            .Join(prologComicBoxes[3].GetComponent<Image>().DOFade(1f, slideDuration))
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                if (prologPageCoroutine == null)
                {
                    StartCoroutine(PlayPrologPageSound());
                }
            })
            .Append(prologComicBoxes[4].GetComponent<RectTransform>().DOAnchorPos(prologScenarios[4].GetEndPoint(), slideDuration))
            .Join(prologComicBoxes[4].GetComponent<Image>().DOFade(1f, slideDuration))
            .AppendInterval(3f)
            .AppendCallback(() => 
            {
                Debug.Log("All pages Out !");
                isAllPageOut = true;
            })
            .Append(blackScreen.DOFade(1f, 1f))
            .AppendInterval(2f)
            .AppendCallback(() => {
                DOTween.KillAll();
                CurrentLevelStatic.lastBuildIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(CurrentLevelStatic.loadingSceneBuildIndex); 
            });
    }

    IEnumerator PlayPrologPageSound()
    {
        prologPageSound.SetActive(true);
        yield return new WaitUntil(prologPageSound.GetComponent<StudioEventEmitter>().IsPlaying);
        prologPageCoroutine = null;
        prologPageSound.SetActive(false);
    }

    IEnumerator PlayPrologSong()
    {
        var studioEventEmitter = prologSound.GetComponent<StudioEventEmitter>();
        studioEventEmitter.EventInstance.setVolume(1f);
        prologSound.SetActive(true);
        yield return new WaitUntil(() => isAllPageOut);
        for (float i = 1f; i >= -1f; i -= 0.1f)
        {
            studioEventEmitter.EventInstance.setVolume(i);
            yield return new WaitForSeconds(0.1f);
        }
        prologSound.SetActive(false);
    }
}
