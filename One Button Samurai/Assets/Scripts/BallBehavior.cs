using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    bool isActive = false;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    void Update()
    {
        if (isActive) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            isActive = true;
        }
    }
}
