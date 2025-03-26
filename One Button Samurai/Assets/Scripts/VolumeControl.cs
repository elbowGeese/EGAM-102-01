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
    private float volumeMin = -80f;

    void Start()
    {
        // set min max
        musicSlider.maxValue = volumeMax;
        musicSlider.minValue = volumeMin;

        sfxSlider.maxValue = volumeMax;
        sfxSlider.minValue = volumeMin;

        Debug.Log(VolumeVariables.currentMusic + "Start");

        // set current volume
        musicSlider.value = VolumeVariables.currentMusic;
        sfxSlider.value = VolumeVariables.currentSFX;

        SetMusicVolume(VolumeVariables.currentMusic);
        SetSFXVolume(VolumeVariables.currentSFX);
    }

    public void SetMusicVolume(float volume)
    {
        volumeMixer.SetFloat("musicVolume", volume);
        VolumeVariables.currentMusic = volume;

        Debug.Log(VolumeVariables.currentMusic + "Set volume");
    }

    public void SetSFXVolume(float volume)
    {
        volumeMixer.SetFloat("sfxVolume", volume);
        VolumeVariables.currentSFX = volume;
    }
}
