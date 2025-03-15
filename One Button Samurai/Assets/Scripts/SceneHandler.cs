using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static event Action onSceneChange;

    // loads a scene based on name given
    public void GoToScene(string sceneName)
    {
        onSceneChange?.Invoke();

        SceneManager.LoadScene(sceneName);
    }

    public void GoToNextLevel()
    {
        onSceneChange?.Invoke();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneManager.GetSceneByBuildIndex(currentSceneIndex + 1) != null)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
