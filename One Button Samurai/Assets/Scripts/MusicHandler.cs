using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    private AudioSource musicSource;

    public float menuSpeed = 0.9f;
    public float mainSpeed = 1.4f;

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            musicSource.pitch = mainSpeed;
        }
        else
        {
            musicSource.pitch = menuSpeed;
        }
    }
}
