using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HitCounter : MonoBehaviour
{
    // vars
    public int hits = 0;
    public int streak = 0;
    public int score = 0;

    public int homerunPoints = 3;
    public int outsidePoints = 2;
    public int insidePoints = 1;
    public int groundballPoints = 0;

    // ui
    public TMP_Text counter;
    private Animator counterAnim;
    public TMP_Text streakCounter;
    private Animator streakAnim;
    public TMP_Text scoreText;
    private Animator scoreAnim;
    public TMP_Text scoreIncreaseText;
    private Animator scoreIncreaseAnim;
    public float timeToIncreaseScore = 1f;

    private void Start()
    {
        SceneHandler.onSceneChange += SaveHitCountToPlayerPrefs;

        counterAnim = counter.gameObject.GetComponent<Animator>();
        streakAnim = streakCounter.gameObject.GetComponent<Animator>();
        scoreAnim = scoreText.gameObject.GetComponent<Animator>();
        scoreIncreaseAnim = scoreIncreaseText.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(counter.text != hits.ToString())
        {
            counter.text = hits.ToString();
            counterAnim.SetTrigger("jump");
        }
        StreakUpdate();
    }

    void StreakUpdate()
    {
        if (streak <= 0)
        {
            streakCounter.text = "";
        }
        else
        {
            if(streakCounter.text != streak.ToString())
            {
                streakCounter.text = streak.ToString();
                streakAnim.SetTrigger("jump");
            }
        }
    }

    public void AddHit(Baseball.HitState hitType)
    {
        hits++;
        streak++;

        int points = 0;
        switch (hitType) 
        { 
            case Baseball.HitState.HOMERUN:
                points = homerunPoints; break;
            case Baseball.HitState.OUTSIDE:
                points = outsidePoints; break;
            case Baseball.HitState.INSIDE:
                points = insidePoints; break;
            case Baseball.HitState.GROUNDBALL:
                points = groundballPoints; break;
        }

        int addScore = points * streak;
        StartCoroutine(IncreaseScore(addScore));
    }

    IEnumerator IncreaseScore(int amount)
    {
        float timePassed = 0f;

        int startScore = score;
        int endScore = score + amount;

        score = endScore;

        int currentScore;

        scoreIncreaseText.text = "+" + amount.ToString();
        scoreIncreaseAnim.SetTrigger("show");

        while (timePassed < timeToIncreaseScore)
        {
            yield return null;

            timePassed += Time.deltaTime;
            currentScore = (int) Mathf.Lerp(startScore, endScore, timePassed / timeToIncreaseScore);
            scoreText.text = currentScore.ToString();
            scoreAnim.SetTrigger("jump");
        }

        scoreText.text = score.ToString();
    }

    public void ResetStreak()
    {
        streak = 0;
        streakAnim.SetTrigger("jump");
    }

    void SaveHitCountToPlayerPrefs()
    {
        // SAVE HIT COUNT
        // save this session's hit count
        PlayerPrefs.SetInt("lastHits", hits);

        // save the best session's hit count
        if(hits > PlayerPrefs.GetInt("bestHits"))
        {
            PlayerPrefs.SetInt("bestHits", hits);
        }

        // SAVE SCORE
        PlayerPrefs.SetInt("lastScore", score);

        if(score > PlayerPrefs.GetInt("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", score);
        }

        // remove listener
        SceneHandler.onSceneChange -= SaveHitCountToPlayerPrefs;
    }
}
