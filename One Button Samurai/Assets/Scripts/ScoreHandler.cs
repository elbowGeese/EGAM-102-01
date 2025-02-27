using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public TMP_Text scoreText;
    public static int currentScore = 0;

    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        currentScore = 0;
    }

    void Update()
    {
        scoreText.text = currentScore.ToString();
    }
}
