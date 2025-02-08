using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public Slider slider;

    public float maxTime = 30f;
    public float timeLeft;

    public bool paused = false;

    void Start()
    {
        paused = false;

        slider.maxValue = maxTime;
        ResetTimer();
    }

    void Update()
    {
        if (paused) { return; }

        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            slider.value = timeLeft;

            if (timeLeft <= 0)
            {
                LoseGame();
            }
        }
    }

    public void ResetTimer()
    {
        timeLeft = maxTime;
        slider.value = timeLeft;
    }

    private void LoseGame()
    {
        GameObject.FindObjectOfType<SceneHandler>().GoToScene("LoseScene");
    }
}
