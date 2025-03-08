using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePadController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float offset = 0.01f;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 ourPos = transform.position;

        Vector3 targetPos = new Vector3(worldPos.x, ourPos.y, ourPos.z);

        if (ourPos.x <= targetPos.x + offset && ourPos.x >= targetPos.x - offset) { return; }

        Vector3 toTargetDelta = targetPos - ourPos;
        Vector3 toTargetDir = toTargetDelta.normalized;

        transform.position += toTargetDir * moveSpeed * Time.deltaTime;
    }
}
