using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLevelUI : MonoBehaviour
{
    public GameObject endLevelUI;
    public TMP_Text bounceScoreText;

    public enum Level { Level1, Level2, Level3 };
    public Level level;

    void Start()
    {
        endLevelUI.SetActive(false);
    }

    public void EndLevel()
    {
        endLevelUI.SetActive(true);

        int endScore = gameObject.GetComponent<Score>()._score;
        bounceScoreText.text = "It took " + endScore.ToString() + " bounces.";

        switch (level)
        {
            case Level.Level1:
                if(endScore < PlayerPrefs.GetInt("Level1Score"))
                {
                    PlayerPrefs.SetInt("Level1Score", endScore);
                }
                break;
            case Level.Level2:
                if (endScore < PlayerPrefs.GetInt("Level2Score"))
                {
                    PlayerPrefs.SetInt("Level2Score", endScore);
                }
                break;
            case Level.Level3:
                if (endScore < PlayerPrefs.GetInt("Level3Score"))
                {
                    PlayerPrefs.SetInt("Level3Score", endScore);
                }
                break;
        }
    }
}
