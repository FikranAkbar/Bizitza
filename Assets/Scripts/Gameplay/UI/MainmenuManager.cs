using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using FMODUnity;

public class MainmenuManager : MonoBehaviour
{
    public Image blackScreen;

    public Button startButton;
    public Button quitButton;

    public GameObject mainMenuSound;
    Coroutine mainMenuCoroutine;

    bool isStartLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuCoroutine = StartCoroutine(PlayMainMenuSong());

        Sequence initSeq = DOTween.Sequence();
        initSeq
            .AppendInterval(2f)
            .Append(blackScreen.DOFade(0f, 1f))
            .AppendCallback(() =>
            {
                startButton.interactable = true;
                quitButton.interactable = true;
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        Sequence loadLevelSeq = DOTween.Sequence();

        loadLevelSeq
            .Append(blackScreen.DOFade(1f, 1f))
            .AppendCallback(() =>
            {
                isStartLevel = true;
            })
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                DOTween.KillAll();
                CurrentLevelStatic.lastBuildIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(CurrentLevelStatic.loadingSceneBuildIndex);
            });
    }

    public void QuitGame()
    {
        startButton.interactable = false;
        quitButton.interactable = false;
        Sequence loadLevelSeq = DOTween.Sequence();

        loadLevelSeq
            .Append(blackScreen.DOFade(1f, 1f))
            .AppendCallback(() =>
            {
                isStartLevel = true;
            })
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                Application.Quit();
            });
    }

    IEnumerator PlayMainMenuSong()
    {
        var studioEventEmitter = mainMenuSound.GetComponent<StudioEventEmitter>();
        studioEventEmitter.EventInstance.setVolume(1f);
        mainMenuSound.SetActive(true);
        print("Before Start Level");
        yield return new WaitUntil(() => isStartLevel);
        print("After Start Level");
        for (float i = 1f; i >= -0.1f; i-=0.1f)
        {
            studioEventEmitter.EventInstance.setVolume(i);
            yield return new WaitForSeconds(0.1f);
        }
        mainMenuSound.SetActive(false);
    }
}
