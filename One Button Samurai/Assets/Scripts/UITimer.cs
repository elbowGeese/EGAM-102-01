using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    public Slider slider;

    public float maxTime = 30f;
    public float timeLeft;

    void Start()
    {
        timeLeft = maxTime;

        slider.maxValue = maxTime;
        slider.value = timeLeft;
    }

    void Update()
    {
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

    private void LoseGame()
    {
        GameObject.FindObjectOfType<SceneHandler>().GoToScene("LoseScene");
    }
}
