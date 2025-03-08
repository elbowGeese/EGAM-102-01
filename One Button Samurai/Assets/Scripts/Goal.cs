using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Transform colTransform;
    public float colMoveSpeed = 1f;

    private void Update()
    {
        if(colTransform != null)
        {
            EatBall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D colRb = collision.gameObject.GetComponent<Rigidbody2D>();
            colRb.velocity = Vector3.zero;
            colRb.gravityScale = 0f;

            colTransform = collision.transform;
        }
    }

    public void EatBall()
    {
        Vector2 toTargetDelta = transform.position - colTransform.position;
        Vector2 toTargetDir = toTargetDelta.normalized;

        colTransform.position += (Vector3) toTargetDir * colMoveSpeed * Time.deltaTime;

        if(colTransform.position.x >= transform.position.x - 0.01f && colTransform.position.x <= transform.position.x + 0.01f)
        {
            if (colTransform.position.y >= transform.position.y - 0.01f && colTransform.position.y <= transform.position.y + 0.01f)
            {
                colTransform = null;
                FindObjectOfType<EndLevelUI>().EndLevel();
            }
        }
    }
}
