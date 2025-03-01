using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject pedestrianPrefab;

    public bool readyToSpawn = false;
    public int currentAmountPedestrians = 1;

    public float[] timeToIncreasePed;
    public float increasePedTimer;

    public float speed = 1f;
    public float speedTimer = 0f;

    public AudioSource backgroundBlip;
    public float fullPitch = 1f;
    public float halfwayPitch = 0.9f;
    public bool playedHalfwayBlip = false;

    public bool gameEnded = false;

    void Start()
    {
        currentAmountPedestrians = 1;
        speedTimer = speed;
        increasePedTimer = timeToIncreasePed[0];

        AddListeners();
    }

    void Update()
    {
        if (gameEnded) { return; }

        // timer to increase # of pedestrians on screen
        if (currentAmountPedestrians <= timeToIncreasePed.Length)
        {
            UpdatePedestrianAmount();
        }

        // find if any pedestrians on screen
        CheckIfEnoughPedestrians();

        // speed timer
        UpdateSpeedTimer();
    }

    public void CheckIfEnoughPedestrians()
    {
        Pedestrian[] peds = FindObjectsOfType<Pedestrian>();
        if (peds.Length < currentAmountPedestrians && readyToSpawn == false)
        {
            readyToSpawn = true;
        }
    }

    public void UpdateSpeedTimer()
    {
        if (speedTimer > 0f)
        {
            speedTimer -= Time.deltaTime;

            // halfway through time
            if (speedTimer <= speed / 2)
            {
                if (!playedHalfwayBlip)
                {
                    backgroundBlip.pitch = halfwayPitch;
                    backgroundBlip.Play();

                    playedHalfwayBlip = true;
                }
            }

            // full way through time
            if (speedTimer <= 0f)
            {
                if (readyToSpawn)
                {
                    Instantiate(pedestrianPrefab);
                    readyToSpawn = false;
                }

                speedTimer = speed;

                backgroundBlip.pitch = fullPitch;
                backgroundBlip.Play();
                playedHalfwayBlip = false;
            }
        }
    }

    public void UpdatePedestrianAmount()
    {
        if (increasePedTimer > 0f)
        {
            increasePedTimer -= Time.deltaTime;

            if(increasePedTimer <= 0f)
            {
                currentAmountPedestrians++;

                if (currentAmountPedestrians <= timeToIncreasePed.Length)
                {
                    increasePedTimer = timeToIncreasePed[currentAmountPedestrians - 1];
                }
            }
        }
    }

    public void AddListeners()
    {
        SceneHandler.onSceneChange += RemoveListeners;
        MissCounter.onGameEnd += OnGameEnd;
    }

    public void RemoveListeners()
    {
        SceneHandler.onSceneChange -= RemoveListeners;
        MissCounter.onGameEnd -= OnGameEnd;
    }

    public void OnGameEnd()
    {
        gameEnded = true;
    }
}
