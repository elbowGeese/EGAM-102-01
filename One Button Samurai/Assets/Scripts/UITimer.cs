using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Animator hourglassAnim;

    public float maxTime = 30f;
    public float timeToColorChange = 10f;
    public float timeToStopColorChange = 8f;
    public Color red;
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

            if (timeLeft <= timeToColorChange)
            {
                fill.color = Color.Lerp(Color.white, red, 1 - ((timeLeft - timeToStopColorChange) / (timeToColorChange - timeToStopColorChange)));
            }

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
        fill.color = Color.white;
    }

    private void LoseGame()
    {
        GameObject.FindObjectOfType<SceneHandler>().GoToScene("LoseScene");
    }
}
