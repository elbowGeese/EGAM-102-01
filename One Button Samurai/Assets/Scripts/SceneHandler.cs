using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    // loads a scene based on name given
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
