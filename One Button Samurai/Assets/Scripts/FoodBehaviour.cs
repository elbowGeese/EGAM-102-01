using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    public bool stolen = false;
    public static event Action onCatSteal;

    public float timeToEnd = 3f;
    public float timer = 0f;

    private void Update()
    {
        if (stolen)
        {
            if (timer < timeToEnd)
            {
                timer += Time.deltaTime;

                if (timer >= timeToEnd)
                {
                    FindObjectOfType<SceneHandler>().GoToScene("LoseScene");
                }
            }
        }
    }

    public void StealFood(Transform cat)
    {
        stolen = true;
        transform.parent = cat;

        onCatSteal?.Invoke();
    }
}
