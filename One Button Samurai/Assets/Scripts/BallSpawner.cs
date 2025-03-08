using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject ballPrefab;

    void Update()
    {
        GameObject sceneBall = GameObject.FindWithTag("Ball");
        if (sceneBall == null)
        {
            GameObject newBall = Instantiate(ballPrefab);
            newBall.transform.position = spawnPos.position;
        }
    }
}
