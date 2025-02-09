using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Android;

public class MoleHandler : MonoBehaviour
{
    // holes
    private HoleBehaviour[] holes;

    // moles
    public int[] maxNumOfMolesInWave;
    private int currentWave = 0;
    public int currentNumOfMoles;

    public GameObject molePrefab;

    // time
    public float maxTimeBetweenMoleSpawn = 2f;
    public float minTimeBetweenMoleSpawn = 0.3f;
    private float timeBetweenMoleSpawn;
    private float moleSpawnTimer;

    public float timeBetweenWaves = 2f;

    private bool paused = false;

    // ui
    public TMP_Text moleCountText;
    public TMP_Text waveText;

    public UITimer uiTimer;

    // sound
    public AudioSource nextWaveSound;

    void Start()
    {
        holes = GameObject.FindObjectsOfType<HoleBehaviour>();
        VariableHolder.waveCount = maxNumOfMolesInWave.Length;
        VariableHolder.waveProgress = currentWave;

        paused = false;
        SetWave(currentWave);

        ResetTimer();
    }

    void Update()
    {
        if (paused) { return; }

        // next wave
        if(currentNumOfMoles <= 0)
        {
            currentWave++;
            VariableHolder.waveProgress = currentWave;

            if(currentWave < maxNumOfMolesInWave.Length)
            {
                StartCoroutine(CallNextWave());
            }
            else
            {
                StartCoroutine(Win());
            }
        }

        // spawn moles
        if(moleSpawnTimer < timeBetweenMoleSpawn)
        {
            moleSpawnTimer += Time.deltaTime;

            if(moleSpawnTimer > timeBetweenMoleSpawn)
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

    IEnumerator CallNextWave()
    {
        nextWaveSound.Play();

        SetWave(currentWave);
        uiTimer.ResetTimer();

        // stop timer
        paused = true;
        uiTimer.paused = true;

        // wait
        yield return new WaitForSeconds(timeBetweenWaves);

        // start next wave
        paused = false;
        uiTimer.paused = false;
    }

    IEnumerator Win()
    {
        nextWaveSound.pitch = 2f;
        nextWaveSound.Play();

        paused = true;
        uiTimer.paused = true;

        yield return new WaitForSeconds(timeBetweenWaves);

        GameObject.FindObjectOfType<SceneHandler>().GoToScene("WinScene");
    }

    private void SetWave(int wave)
    {
        waveText.text = "WAVE: " + (wave + 1);

        currentNumOfMoles = maxNumOfMolesInWave[wave];
        SetMoleText();
    }

    private void SetMoleText()
    {
        moleCountText.text = "Moles: " + currentNumOfMoles;
    }

    private void ResetTimer()
    {
        timeBetweenMoleSpawn = Random.Range(minTimeBetweenMoleSpawn, maxTimeBetweenMoleSpawn);
        moleSpawnTimer = 0f;
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

    public void RemoveMole()
    {
        currentNumOfMoles--;
        SetMoleText();
    }
}
