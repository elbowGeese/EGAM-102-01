using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Update()
    {
        if (IsOutsideBounds())
        {
            Destroy(gameObject);
        }
    }

    public bool IsOutsideBounds()
    {
        if (transform.position.x < minX || transform.position.x > maxX) { return true; }

        if(transform.position.y < minY || transform.position.y > maxY) {  return true; }

        return false;
    }
}
