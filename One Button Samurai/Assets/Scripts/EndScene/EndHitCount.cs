using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndHitCount : MonoBehaviour
{
    public TMP_Text lastCounter;
    public TMP_Text bestCounter;

    public TMP_Text lastScore;
    public TMP_Text bestScore;

    void Start()
    {
        lastCounter.text = "YOU HIT " + PlayerPrefs.GetInt("lastHits") + " BASEBALLS!";
        bestCounter.text = "BEST HITS: " + PlayerPrefs.GetInt("bestHits");

        lastScore.text = "YOUR SCORE: " + PlayerPrefs.GetInt("lastScore").ToString();
        bestScore.text = "BEST SCORE: " + PlayerPrefs.GetInt("bestScore").ToString();
    }
}
