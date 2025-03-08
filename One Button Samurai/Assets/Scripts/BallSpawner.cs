using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject ballPrefab;
    public float force = 5f;

    void Update()
    {
        GameObject sceneBall = GameObject.FindWithTag("Ball");
        if (sceneBall == null)
        {
            SpawnBall();
        }
    }

    public void SpawnBall()
    {
        GameObject newBall = Instantiate(ballPrefab);
        newBall.transform.position = spawnPos.position;

        newBall.GetComponent<Rigidbody2D>().AddForce(-spawnPos.up * force, ForceMode2D.Impulse);
    }
}
