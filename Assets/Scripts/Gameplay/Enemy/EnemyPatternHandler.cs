using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class EnemyPatternHandler : MonoBehaviour
{
    [SerializeField] GameObject patternDrawerPrefab;
    public Queue<GameObject> enemyPatterns;
    public GestureRecognizer.DrawDetector drawDetector;
    private GameObject enemyParent;

    private EnemyController enemyController;

    public static bool isThereAnyCorrentPattern;

    public GameObject enemyGetHitSound;
    Coroutine enemyGetHitCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        drawDetector = FindObjectOfType<GestureRecognizer.DrawDetector>();
        enemyParent = transform.parent.parent.parent.gameObject;
        enemyController = enemyParent.transform.GetChild(0).GetComponent<EnemyController>();
        StartCoroutine(AddListenerToDrawDetector());

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyPatterns != null)
        {
            if (enemyPatterns.Count == 0)
            {
                if (enemyController.enemyType == "Boss")
                {
                    enemyController.enemyBehaviour = null;
                    enemyController.enemyType = "";
                }
                else if (enemyController.isAlive == false)
                {
                    drawDetector.OnRecognize.RemoveListener(RemovePatternIfMatch);
                    enemyController.PlayDeadAnim();
                    if (enemyPatterns != null)
                    {
                        enemyPatterns.Clear();
                        foreach (Transform element in transform)
                        {
                            Destroy(element.transform.gameObject);
                        }
                    }
                    Destroy(this);
                }
            }
        }
    }

    public void DrawPattern(List<GestureRecognizer.GesturePattern> gestures)
    {
        enemyPatterns = new Queue<GameObject>();
        foreach (var gesture in gestures)
        {
            GameObject patternDraw = Instantiate(patternDrawerPrefab, transform);
            patternDraw.GetComponent<GestureRecognizer.GesturePatternDraw>().pattern = gesture;
            enemyPatterns.Enqueue(patternDraw);
        }

        // StartCoroutine(AddListenerToDrawDetector());
    }

    IEnumerator AddListenerToDrawDetector()
    {
        yield return new WaitUntil(() => drawDetector != null);
        drawDetector.OnRecognize.AddListener(RemovePatternIfMatch);
    }

    public void HideAllPattern()
    {
        foreach (var item in enemyPatterns)
        {
            item.SetActive(false);
        }
        enemyController.gestures = null;
    }

    public void RemovePatternIfMatch(GestureRecognizer.RecognitionResult result)
    {
        var obj = enemyPatterns.Peek();
        GestureRecognizer.GesturePattern gesture = obj.GetComponent<GestureRecognizer.GesturePatternDraw>().pattern;
        if (gesture != null && result != GestureRecognizer.RecognitionResult.Empty)
        {
            if (gesture.id == result.gesture.id && enemyController.isAlive)
            {
                if (enemyGetHitCoroutine == null) // && SceneManager.GetActiveScene().buildIndex != CurrentLevelStatic.levelTrainingBuildIndex)
                {
                    enemyGetHitCoroutine = StartCoroutine(PlayEnemyGetHitSound());
                }
                enemyController.PlayHitAnim();
                var enemyPattern = enemyPatterns.Dequeue();
                Destroy(enemyPattern);
                if (ScoreCounter.addScoreDelegate != null && SceneManager.GetActiveScene().buildIndex != CurrentLevelStatic.levelTrainingBuildIndex)
                {
                    ScoreCounter.addScoreDelegate.Invoke();
                }
                if (enemyPatterns.Count <= 0)
                {
                    enemyController.isAlive = false;
                }
            }
        }
    } 

    IEnumerator PlayEnemyGetHitSound()
    {
        enemyGetHitSound.SetActive(true);
        yield return new WaitUntil(enemyGetHitSound.GetComponent<StudioEventEmitter>().IsPlaying);
        enemyGetHitCoroutine = null;
        enemyGetHitSound.SetActive(false);
    }
}
