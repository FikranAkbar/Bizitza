using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneHandler : MonoBehaviour
{
    public Image loadingGhost;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
            .Append(loadingGhost.DOFade(1f, 0.5f))
            .AppendInterval(0.5f)
            .Append(loadingGhost.DOFade(0f, 0.5f))
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene(CurrentLevelStatic.lastBuildIndex + 1);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
