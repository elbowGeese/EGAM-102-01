using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    public GameObject baseballPrefab;
    public float bufferSpawnTime = 1f;
    private bool isSpawning = false;

    void Update()
    {
        // paused
        if (PauseHandler.instance.pauseMenu.activeSelf) { return; }

        // unpaused
        Baseball baseball = FindAnyObjectByType<Baseball>();
        if(baseball == null && !isSpawning)
        {
            StartCoroutine(SpawnBaseball());
        }
    }

    IEnumerator SpawnBaseball()
    {
        isSpawning = true;

        yield return new WaitForSeconds(bufferSpawnTime);

        GameObject baseball = Instantiate(baseballPrefab);
        baseball.transform.position = transform.position;

        isSpawning = false;
    }
}
