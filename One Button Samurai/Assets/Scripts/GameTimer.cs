using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeToCook = 120f;
    private float timer;
    public float timeToWin = 3f;

    public static event Action onFinishedCooking;

    public Slider slider;

    void Start()
    {
        timer = 0f;

        slider.maxValue = timeToCook;
        slider.value = timer;
    }

    void Update()
    {
        if (timer < timeToCook)
        {
            timer += Time.deltaTime;
            slider.value = timer;

            if(timer >= timeToCook)
            {
                onFinishedCooking?.Invoke();
                StartCoroutine(FinishedCooking());
            }
        }
    }

    IEnumerator FinishedCooking()
    {
        yield return new WaitForSeconds(timeToWin);

        FindObjectOfType<SceneHandler>().GoToScene("WinScene");
    }
}
