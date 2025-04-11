using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    public GameObject baseballPrefab;
    public AnimationCurve bufferSpawnTime;
    private bool isSpawning = false;

    private HitCounter hitCounter;

    private void Start()
    {
        hitCounter = FindFirstObjectByType<HitCounter>();
    }

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

        yield return new WaitForSeconds(bufferSpawnTime.Evaluate(hitCounter.streak));

        GameObject baseball = Instantiate(baseballPrefab);
        baseball.transform.position = transform.position;

        isSpawning = false;
    }
}
