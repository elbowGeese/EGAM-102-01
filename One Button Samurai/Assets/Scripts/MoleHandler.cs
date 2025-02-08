using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoleHandler : MonoBehaviour
{
    // holes
    private HoleBehaviour[] holes;

    // moles
    public int maxNumOfMoles = 5;
    public int currentNumOfMoles;

    public GameObject molePrefab;

    // time
    public float maxTimeBetweenMoleSpawn = 2f;
    public float minTimeBetweenMoleSpawn = 0.3f;
    private float timeBetweenMoleSpawn;
    private float timer;

    // ui
    public TMP_Text moleCountText;

    void Start()
    {
        holes = GameObject.FindObjectsOfType<HoleBehaviour>();

        currentNumOfMoles = maxNumOfMoles;

        ResetTimer();
        SetMoleText();
    }

    void Update()
    {
        if(timer < timeBetweenMoleSpawn)
        {
            timer += Time.deltaTime;

            if(timer > timeBetweenMoleSpawn)
            {
                // add mole
                if (MoleCount() < currentNumOfMoles)
                {
                    AddMole();
                }

                // reset timer
                ResetTimer();
            }
        }
    }

    private void SetMoleText()
    {
        moleCountText.text = "Moles: " + currentNumOfMoles;
    }

    private void ResetTimer()
    {
        timeBetweenMoleSpawn = Random.Range(minTimeBetweenMoleSpawn, maxTimeBetweenMoleSpawn);
        timer = 0f;
    }

    private int MoleCount()
    {
        int moles = 0;

        foreach (var hole in holes)
        {
            if (hole.hasMole)
            {
                moles++;
            }
        }

        return moles;
    }

    private int GetOpenHoleIndex()
    {
        int holeIndex = Random.Range(0, holes.Length);

        // choose a hole that doesnt have a mole
        while (holes[holeIndex].hasMole)
        {
            holeIndex = Random.Range(0, holes.Length);
        }

        return holeIndex;
    }

    private void AddMole()
    {
        // choose hole
        int holeIndex = GetOpenHoleIndex();

        // spawn mole
        GameObject newMole = Instantiate(molePrefab, holes[holeIndex].transform);
        holes[holeIndex].hasMole = true;
    }
}
