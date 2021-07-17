using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using DG.Tweening;

public class HealthCounter : MonoBehaviour
{
    public delegate void AddHealthDelegate();
    public static AddHealthDelegate addHealthDelegate;

    public delegate void DecreaseHealthDelegate();
    public static DecreaseHealthDelegate decreaseHealthDelegate;

    public GameObject GameOverPanel;

    public GameObject healthUnitPrefab;
    public int healthTotal;

    public GameObject gameOverSound;
    Coroutine gameOverCoroutine;

    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        decreaseHealthDelegate += DecreaseHealth;
        for (int i = 0; i < healthTotal; i++)
        {
            Instantiate(healthUnitPrefab, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseHealth()
    {
        if (transform.childCount > 1) {
            transform.GetChild(transform.childCount - 1).gameObject.GetComponent<HealthUnit>().PlayDecreaseHealthAnimation();
        } 
        
        else
        {
            transform.GetChild(transform.childCount - 1).gameObject.GetComponent<HealthUnit>().PlayDecreaseHealthAnimation();

            GameOver();
        }
    }

    public void GameOver()
    {
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                StartCoroutine(PlayGameOverSound());
                LevelManager.isLevelFailed = true;
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                levelManager.PlayGameOverAnimation();
            });
    }

    IEnumerator PlayGameOverSound()
    {
        var studioEventEmitter = gameOverSound.GetComponent<StudioEventEmitter>();
        studioEventEmitter.EventInstance.setVolume(1f);
        gameOverSound.SetActive(true);
        yield return new WaitUntil(() => LevelManager.isTransitioningTo);
        for (float i = 1f; i > 0f; i -= 0.1f)
        {
            if (i < 0f)
                i = 0f;
            studioEventEmitter.EventInstance.setVolume(i);
            yield return new WaitForSeconds(0.1f);
        }
        gameOverCoroutine = null;
        gameOverSound.SetActive(false);
    }
}
