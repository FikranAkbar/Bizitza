using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ToBeContinuedHandler : MonoBehaviour
{

    public TextMeshProUGUI toBeContText;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
            .Append(toBeContText.DOFade(1f, 1f))
            .AppendInterval(1.5f)
            .Append(toBeContText.DOFade(0f, 1f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
