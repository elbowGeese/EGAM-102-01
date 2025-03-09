using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BouncePadController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float offset = 0.01f;
    public float leftRightForce = 1f;

    private Score score;

    private void Start()
    {
        score = FindObjectOfType<Score>();
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            score._score += 1;

            if (BallIsLeft(collision.transform))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * leftRightForce, ForceMode2D.Impulse);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * leftRightForce, ForceMode2D.Impulse);
            }
        }
    }

    public bool BallIsLeft(Transform ballTransform)
    {
        if (ballTransform.position.x > transform.position.x)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
