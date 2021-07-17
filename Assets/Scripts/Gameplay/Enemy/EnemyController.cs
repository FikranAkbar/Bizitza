using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

[System.Serializable] public class _UnityEventGestures : UnityEvent<List<GestureRecognizer.GesturePattern>> { }
[System.Serializable] public class _UnityEventEnemyDead : UnityEvent { }

public class EnemyController : MonoBehaviour
{
    public EnemyBehaviour enemyBehaviour;
    public string enemyType;
    public List<GestureRecognizer.GesturePattern> gestures = null;
    List<List<GestureRecognizer.GesturePattern>> listGestures = null;
    SpriteRenderer spriteRenderer;
    public Animator animator;
    Vector2 target;
    public bool isAttackingPlayer = false;
    public bool isAttacked = false;
    public bool isAlive = false;
    public bool isSpawned = false;

    public float maxDistanceToPlayer = 0.5f;

    public _UnityEventGestures assignPattern;
    public _UnityEventEnemyDead hidePatterns;
    public ScoreCounter scoreCounter;
    public HealthCounter healthCounter;

    public GameObject playerGetHitSound;
    Coroutine playerGetHitCoroutine;

    private Canvas trainingCanvas;
    private TutorialScenario tutorialScenarioController;

    bool isTutorialTriggered = false;


    Image redScreen;

    Animator playerAnimator;

    public int bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Training Canvas") != null)
        {
            trainingCanvas = GameObject.FindGameObjectWithTag("Training Canvas").GetComponent<Canvas>();
            tutorialScenarioController = GameObject.FindGameObjectWithTag("Training Canvas").GetComponent<TutorialScenario>();
        }

        StartCoroutine(StartMoving());
        scoreCounter = FindObjectOfType<ScoreCounter>();
        healthCounter = FindObjectOfType<HealthCounter>();

        float playerPos = GameObject.FindGameObjectWithTag("Player").transform.position.x;

        if (playerPos > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }

        var mainUICanvas = GameObject.FindGameObjectWithTag("Main UI Canvas");
        redScreen = mainUICanvas.transform.GetChild(mainUICanvas.transform.childCount - 1).GetComponent<Image>();

        var player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target) > maxDistanceToPlayer && !LevelManager.isLevelFailed)
        {
            if (isAlive && isSpawned && !isAttacked)
            {
                MoveTorwardsPlayer();
            }
        }
        else if (Vector2.Distance(transform.position, target) <= maxDistanceToPlayer && !isTutorialTriggered && SceneManager.GetActiveScene().buildIndex == CurrentLevelStatic.levelTrainingBuildIndex)
        {
            Debug.Log("Tutorial Triggered");
            isTutorialTriggered = true;
            trainingCanvas.enabled = true;
            tutorialScenarioController.enabled = true;
        }
    }

    void MoveTorwardsPlayer()
    {
        float step;
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        if (enemyBehaviour != null)
        {
            step = enemyBehaviour.GetMoveSpeed() * Time.deltaTime;
        }
        else
        {
            step = 1f * Time.deltaTime;
        }
        if (asi.IsName("Base Layer.Hit_Ghost") && asi.IsName("Base Layer.Hit_Boss_1"))
        {
            step = 0f;
        }

        transform.parent.position = Vector2.MoveTowards(transform.position, target, step);
        print("Step: " + step);
        print("Target: " + target);
    }

    void InitEnemyProperty()
    {
        // Setup target
        var player = FindObjectOfType<PlayerController>().transform.parent;
        target = player.position;

        // Setup sprite
        spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = enemyBehaviour.GetEnemySprite();

        // Setup gestures
        if (enemyBehaviour != null)
        {
            print("Invoke assignPattern");
            gestures = enemyBehaviour.GetGestures();
            assignPattern.Invoke(gestures);
        }
         
        // Setup animation
        animator = transform.parent.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = enemyBehaviour.GetEnemyAnimator();
    }

    IEnumerator StartMoving()
    {
        yield return new WaitUntil(() => gestures != null);
        isSpawned = true;
    }

    public void AssignEnemyBehaviour(EnemyBehaviour enemyBehaviour)
    {
        this.enemyBehaviour = enemyBehaviour;
        isAlive = true;
        InitEnemyProperty();
    }

    /*
    public void AssignEnemyBehaviour(List<BossBehaviour> bossesBehaviour)
    {
        bossBehaviours = new List<BossBehaviour>(2);
        for(int i = 0; i < bossesBehaviour.Count; i++)
        {
            bossBehaviours[i] = bossesBehaviour[i];
            isAlive = true;
        }
    }
    */

    private void OnDestroy()
    {
        LevelManager.enemyCount--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DOTween.Sequence()
                .Append(redScreen.DOFade(110f / 256f, 0.3f))
                .Append(redScreen.DOFade(0f, 0.3f));
            playerGetHitCoroutine = StartCoroutine(PlayPlayerGetHitSound());
            healthCounter.DecreaseHealth();
            scoreCounter.ResetScoreMultiplier();

            if (enemyType == "Boss")
            {
                PlayBossAttackAnim();
            }
            else
            {
                PlayAttackAnim();
            }
        }
    }

    public void PlayHitAnim()
    {
        isAttacked = true;
        animator.Play("Base Layer.Hit_Ghost", 0, 0f);
    }

    public void PlayDeadAnim()
    {
        isAlive = false;
        animator.Play("Base Layer.Dead_Ghost");
    }

    public void PlayAttackAnim()
    {
        isAlive = false;
        animator.Play("Base Layer.Attack_Ghost");
        playerAnimator.Play("Base Layer.Hit_Player", 0, 0);
        hidePatterns.Invoke();
    }

    public void PlayBossAttackAnim()
    {
        animator.Play("Base Layer.Boss_Attack_Ghost");
        playerAnimator.Play("Base Layer.Hit_Player");
        //hidePatterns.Invoke();
        //enemyType = "";

    }

    public void PlayIdleAnim()
    {
        isAttacked = false;
        animator.Play("Base Layer.Idle_Ghost");
    }

    public void PushEnemy()
    {
        transform.parent.DOMoveX(8f, 0.2f);
    }

    IEnumerator PlayPlayerGetHitSound()
    {
        playerGetHitSound.SetActive(true);
        yield return new WaitUntil(playerGetHitSound.GetComponent<StudioEventEmitter>().IsPlaying);
        playerGetHitCoroutine = null;
        playerGetHitSound.SetActive(false);
    }
}
