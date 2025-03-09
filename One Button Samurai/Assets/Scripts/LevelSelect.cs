using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    public TMP_Text level1ScoreText;
    public TMP_Text level2ScoreText;
    public TMP_Text level3ScoreText;

    void Start()
    {
        int level1 = PlayerPrefs.GetInt("Level1Score");
        int level2 = PlayerPrefs.GetInt("Level2Score");
        int level3 = PlayerPrefs.GetInt("Level3Score");

        level1ScoreText.text = "Best Score: " + level1;
        level2ScoreText.text = "Best Score: " + level2;
        level3ScoreText.text = "Best Score: " + level3;
    }

}
