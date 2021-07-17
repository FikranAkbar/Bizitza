using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public delegate void AddScoreDelegate();
    public static AddScoreDelegate addScoreDelegate;

    public delegate void ResetScoreMultiplierDelegate();
    public static ResetScoreMultiplierDelegate resetScoreMultiplierDelegate;

    public static int scoreMultiplier = 0;
    public static int score;
    public float growthDelay = 0.04f;
    private float _growthDelay;
    public int scoreInc = 40;

    private static float timer;
    public float maxTimer = 2.5f;

    private Coroutine scoreMultiplierCoroutine;

    private Text scoreMultiplierText;
    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreMultiplierText = transform.GetChild(0).GetComponent<Text>();
        scoreText = transform.GetChild(1).GetComponent<Text>();

        addScoreDelegate += AddScore;
        resetScoreMultiplierDelegate    += ResetScoreMultiplier;

        _growthDelay = growthDelay;

        score = ScoreUnit.score;
        scoreText.text = ScoreUnit.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        scoreMultiplierText.gameObject.SetActive(scoreMultiplier > 1);
        _growthDelay -= Time.deltaTime;
        if (int.Parse(scoreText.text) < score && _growthDelay <= 0)
        {
            _growthDelay = growthDelay;
            var currentScore = int.Parse(scoreText.text);
            currentScore++;
            scoreText.text = currentScore.ToString();
        }
    }

    public void AddScore()
    {
        scoreMultiplier += 1;
        score += (scoreInc * scoreMultiplier);

        if (scoreMultiplier != 1)
        {
            WriteScore();
        }

        timer = maxTimer;
        StartCoroutine(ResetScoreMultiplierUntilSeconds());
        WriteMultiplierScore();
    }

    public void ResetScoreMultiplier()
    {
        scoreMultiplier = 0;
        WriteMultiplierScore();
    }

    private IEnumerator ResetScoreMultiplierUntilSeconds()
    {
        yield return new WaitUntil(() => timer <= 0f);
        scoreMultiplier = 0;
        WriteMultiplierScore();
        StopAllCoroutines();
    }

    private void WriteScore()
    {

        scoreText.text = (score - scoreInc).ToString();
        //scoreText.text = "Score: " + score.ToString();
    }

    private void WriteMultiplierScore()
    {
        
        scoreMultiplierText.text = "x" + scoreMultiplier.ToString();
        //scoreMultiplierText.text = "Multiplier: " + scoreMultiplier.ToString();
    }
}
