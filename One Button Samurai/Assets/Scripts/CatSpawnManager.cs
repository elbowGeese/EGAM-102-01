using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawnManager : MonoBehaviour
{
    public float leftXPos;
    public float rightXPos;

    public GameObject catPrefab;

    public int startingCats = 3;

    public float timeToSpawnCat = 2f;
    private float timer = 0f;

    private bool paused = false;

    private void Start()
    {
        for (int i = 0; i < startingCats; i++)
        {
            SpawnCat();
        }
    }

    void Update()
    {
        if (paused) { return; }

        if (timer < timeToSpawnCat)
        {
            timer += Time.deltaTime;

            if (timer >= timeToSpawnCat)
            {
                SpawnCat();
                timer = 0f;
            }
        }
    }

    public void SpawnCat()
    {
        // choose position
        Vector2 spawnPos;

        bool left = Random.Range(0, 2) == 0;
        if (left)
        {
            spawnPos = new Vector2(leftXPos, VariableStorage.groundY);
        }
        else
        {
            spawnPos = new Vector2(rightXPos, VariableStorage.groundY);
        }

        // spawn cat at position
        GameObject cat = Instantiate(catPrefab);
        cat.transform.position = spawnPos;
    }

    public void AddListeners()
    {
        SceneHandler.onSceneChange += RemoveListeners;
        FoodBehaviour.onCatSteal += PauseCatSpawner;
        GameTimer.onFinishedCooking += PauseCatSpawner;
    }

    public void RemoveListeners()
    {
        SceneHandler.onSceneChange -= RemoveListeners;
        FoodBehaviour.onCatSteal -= PauseCatSpawner;
        GameTimer.onFinishedCooking -= PauseCatSpawner;
    }

    public void PauseCatSpawner()
    {
        paused = true;
    }
}
