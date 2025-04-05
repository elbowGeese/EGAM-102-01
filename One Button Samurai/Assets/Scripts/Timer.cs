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
        // paused
        if (PauseHandler.instance.pauseMenu.activeSelf) { return; }

        // unpaused
        if (secondsPassed > 0f)
        {
            secondsPassed -= Time.unscaledDeltaTime;

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
