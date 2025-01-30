using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;

    void Start()
    {
        ClosePauseMenu();
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
        pauseButton.SetActive(true);

        Time.timeScale = 1f;
    }
}
