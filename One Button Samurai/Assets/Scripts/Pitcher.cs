using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitcher : MonoBehaviour
{
    public GameObject fastballPrefab;
    public int fastballWeight = 5;
    public GameObject curveballPrefab;
    public int curveballWeight = 2;
    public GameObject screwballPrefab;
    public int screwballWeight = 2;
    public GameObject splitterPrefab;
    public int splitterWeight = 1;

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

        GameObject baseball = Instantiate(ChooseBaseball());
        baseball.transform.position = transform.position;

        isSpawning = false;
    }

    private GameObject ChooseBaseball()
    {
        List<GameObject> baseballList = new List<GameObject>();

        for(int i = 0; i < fastballWeight; i++)
        {
            baseballList.Add(fastballPrefab);
        }

        for (int i = 0; i < curveballWeight; i++)
        {
            baseballList.Add(curveballPrefab);
        }

        for (int i = 0; i < screwballWeight; i++)
        {
            baseballList.Add(screwballPrefab);
        }

        for (int i = 0; i < splitterWeight; i++)
        {
            baseballList.Add(splitterPrefab);
        }

        return baseballList[Random.Range(0, baseballList.Count)];
    }
}
