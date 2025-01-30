using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;

    void Start()
    {
        ClosePauseMenu();
        pauseButton.SetActive(false);

        CountdownHandler.onCountdownDone += OpenPauseButton;
    }

    // opens the pause menu and hides the pause button
    // pauses game
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);

        Time.timeScale = 0f;
    }

    // closes the pause menu and shows the pause button
    // resumes game
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
    }

    // called onCountdownDone
    public void OpenPauseButton()
    {
        pauseButton.SetActive(true);
    }

    // fucking unsubscribe dont throw errors on scene change
    public void DisableCountdownListener()
    {
        CountdownHandler.onCountdownDone -= OpenPauseButton;
    }
}
