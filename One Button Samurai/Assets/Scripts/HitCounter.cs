using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitCounter : MonoBehaviour
{
    public int hits = 0;
    public int streak = 0;
    public TMP_Text counter;
    public TMP_Text streakCounter;

    private void Start()
    {
        SceneHandler.onSceneChange += SaveHitCountToPlayerPrefs;
    }

    void Update()
    {
        counter.text = hits.ToString();
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
            streakCounter.text = streak.ToString();
        }
    }

    public void AddHit()
    {
        hits++;
        streak++;
    }

    public void ResetStreak()
    {
        streak = 0;
    }

    void SaveHitCountToPlayerPrefs()
    {
        // save this session's hit count
        PlayerPrefs.SetInt("lastHits", hits);

        // save the best session's hit count
        if(hits > PlayerPrefs.GetInt("bestHits"))
        {
            PlayerPrefs.SetInt("bestHits", hits);
        }

        SceneHandler.onSceneChange -= SaveHitCountToPlayerPrefs;
    }
}
