using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chronometer : MonoBehaviour
{
    public float scale = 0.1f;

    public float minTimeScale = 0.1f;
    public float maxTimeScale = 2f;
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxTimeScale;
        slider.minValue = minTimeScale;
    }

    void Update()
    {
        // paused
        if (PauseHandler.instance.pauseMenu.activeSelf) { return; }

        // not paused
        float currentTimeScale = Time.timeScale + (Input.mouseScrollDelta.y * scale);
        if(currentTimeScale > maxTimeScale) { currentTimeScale = maxTimeScale; }
        if(currentTimeScale < minTimeScale) { currentTimeScale = minTimeScale; }
        Time.timeScale = currentTimeScale;
        slider.value = currentTimeScale;
    }
}
