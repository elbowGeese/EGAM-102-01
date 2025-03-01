using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissCounter : MonoBehaviour
{
    public int currentMisses = 0;

    public GameObject missText;
    public GameObject missImage1;
    public GameObject missImage2;
    public GameObject missImage3;

    public AudioSource missSFX;

    public static event Action onGameEnd;

    void Start()
    {
        ResetMisses();
    }

    private void Update()
    {
        if (currentMisses > 0)
        {
            if (!missText.activeSelf)
            {
                missText.SetActive(true);
                missSFX.Play();
            }

            if (!missImage1.activeSelf)
            {
                missImage1.SetActive(true);
                missSFX.Play();
            }

            if (currentMisses > 1 && !missImage2.activeSelf)
            {
                missImage2.SetActive(true);
                missSFX.Play();
            }

            if (currentMisses > 2 && !missImage3.activeSelf)
            {
                missImage3.SetActive(true);
                EndGame();
            }
        }
    }

    public void ResetMisses()
    {
        currentMisses = 0;

        missText.SetActive(false);
        missImage1.SetActive(false);
        missImage2.SetActive(false);
        missImage3.SetActive(false);
    }

    public void EndGame()
    {
        Debug.Log("GAME END");
        onGameEnd?.Invoke();
    }
}
