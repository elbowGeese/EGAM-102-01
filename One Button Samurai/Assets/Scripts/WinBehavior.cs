using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBehavior : MonoBehaviour
{
    public bool isReady = false;
    public float waitForReady = 3f;

    void Start()
    {
        StartCoroutine(BecomeReady());
    }

    void Update()
    {
        if (!isReady) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<SceneHandler>().GoToScene("MenuScene");
        }
    }

    IEnumerator BecomeReady()
    {
        yield return new WaitForSeconds(waitForReady);
        isReady = true;
    }
}
