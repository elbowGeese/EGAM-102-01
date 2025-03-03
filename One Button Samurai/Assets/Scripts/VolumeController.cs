using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer volumeMixer;
    public Slider slider;

    public float maxVolume = 0f;
    public float minVolume = -80f;

    void Start()
    {
        slider.minValue = minVolume;
        slider.maxValue = maxVolume;

        slider.value = maxVolume;
    }

    private void Update()
    {
        volumeMixer.SetFloat("masterVolume", slider.value);
    }
}
