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

    private AudioSource beep;
    public bool[] playedCountdownBeep; // length of array is the number of seconds before the end of the timer, plays every second for the countdown

    private void Start()
    {
        beep = GetComponent<AudioSource>();

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

            PlayCountDownBeeps();

            if(secondsPassed <= 0f)
            {
                timeLeft.text = "00:00";
                FindFirstObjectByType<SceneHandler>().GoToScene("WinScene");
            }
        }
    }

    private void PlayCountDownBeeps()
    {
        for(int i = 1; i < playedCountdownBeep.Length; i++)
        {
            if(secondsPassed <= (float)i && playedCountdownBeep[i] == false)
            {
                if (i == 1) { beep.pitch = 1.3f; }
                beep.Play();
                playedCountdownBeep[i] = true;
            }
        }
    }
}
