using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float secondsToPlay = 100f;
    private float secondsPassed = 0f;
    public TMP_Text timeLeft;

    private void Start()
    {
        secondsPassed = secondsToPlay;
    }

    void Update()
    {
        if (secondsPassed > 0f)
        {
            secondsPassed -= Time.deltaTime; // will probably change this later to system time instead of game time

            int minutes = TimeSpan.FromSeconds(secondsPassed).Minutes;
            int seconds = (int) secondsPassed - (minutes * 60);
            timeLeft.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if(secondsPassed <= 0f)
            {
                timeLeft.text = "00:00";
                FindFirstObjectByType<SceneHandler>().GoToScene("WinScene");
            }
        }
    }
}
