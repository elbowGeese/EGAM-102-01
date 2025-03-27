using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer volumeMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private float volumeMax = 0f;
    private float volumeMin = -60f;

    void Start()
    {
        // set min max
        musicSlider.maxValue = volumeMax;
        musicSlider.minValue = volumeMin;

        sfxSlider.maxValue = volumeMax;
        sfxSlider.minValue = volumeMin;

        // set current volume
        musicSlider.value = VolumeVariables.currentMusic;
        sfxSlider.value = VolumeVariables.currentSFX;

        AddListeners();
    }

    public void AddListeners()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        volumeMixer.SetFloat("musicVolume", volume);
        VolumeVariables.currentMusic = volume;
    }

    public void SetSFXVolume(float volume)
    {
        volumeMixer.SetFloat("sfxVolume", volume);
        VolumeVariables.currentSFX = volume;
    }
}
