using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessMoleHandler : MonoBehaviour
{
    // holes
    private HoleBehaviour[] holes;

    // moles
    public GameObject molePrefab;

    // time
    public AnimationCurve timeBetweenMoleSpawn;
    private float nextMoleSpawn;
    private float moleSpawnTimer;
    private float overallTime;
    public float endWaitTime = 2f;

    private bool paused = false;

    // ui
    public TMP_Text moleCountText;
    private int molesWhacked = 0;
    public MissCounter[] missCounters;
    private int molesMissed = 0;

    // sound
    public AudioSource nextWaveSound;

    void Start()
    {
        holes = GameObject.FindObjectsOfType<HoleBehaviour>();

        nextMoleSpawn = timeBetweenMoleSpawn.Evaluate(overallTime);
        paused = false;
        UpdateMissMoleCounter();
        SetMoleText();
    }

    void Update()
    {
        if (paused) { return; }

        overallTime += Time.deltaTime;

        // spawn moles
        if (moleSpawnTimer < nextMoleSpawn)
        {
            moleSpawnTimer += Time.deltaTime;

            if (moleSpawnTimer >= nextMoleSpawn)
            {
                // add mole
                AddMole();
                moleSpawnTimer = 0f;
                nextMoleSpawn = timeBetweenMoleSpawn.Evaluate(overallTime);
            }
        }
    }

    IEnumerator EndGame()
    {
        nextWaveSound.pitch = 2f;
        nextWaveSound.Play();

        paused = true;
        // gonna need to pause some more stuff

        yield return new WaitForSeconds(endWaitTime);

        EndlessModeHitCountVariable.hitCount = molesWhacked;

        GameObject.FindObjectOfType<SceneHandler>().GoToScene("EndlessEndScene");
    }


    private void SetMoleText()
    {
        moleCountText.text = "HITS: " + molesWhacked;
    }

    private int GetOpenHoleIndex()
    {
        int holeIndex = Random.Range(0, holes.Length);

        // choose a hole that doesnt have a mole
        int tries = 20;
        while (tries > 0)
        {
            holeIndex = Random.Range(0, holes.Length);
            if (!holes[holeIndex].hasMole)
            {
                if (!holes[holeIndex].onPlayer)
                {
                    return holeIndex;
                }
            }

            tries--;
        }

        return -1;
    }

    private void AddMole()
    {
        // choose hole
        int holeIndex = GetOpenHoleIndex();
        if (holeIndex < 0) { return; }

        // spawn mole
        GameObject newMole = Instantiate(molePrefab, holes[holeIndex].transform);
        holes[holeIndex].hasMole = true;
    }

    public void RemoveMole()
    {
        molesWhacked++;
        SetMoleText();
    }

    public void MissMole()
    {
        molesMissed++;
        UpdateMissMoleCounter();
    }

    private void UpdateMissMoleCounter()
    {
        switch (molesMissed)
        {
            case 0:
                missCounters[0].Deactivate();
                missCounters[1].Deactivate();
                missCounters[2].Deactivate();
                break;
            case 1:
                missCounters[0].Activate();
                break;
            case 2:
                missCounters[1].Activate();
                break;
            case 3:
                missCounters[2].Activate();
                StartCoroutine(EndGame());
                break;
        }
    }
}
