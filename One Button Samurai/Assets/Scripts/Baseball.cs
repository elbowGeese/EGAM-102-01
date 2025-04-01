using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    public AnimationCurve sizeOverTime;
    public AnimationCurve rotationOverTime;

    public float timeAlive = 4f;
    public float timeToFadeOut = 0f;
    private float timePassed;

    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        timePassed = 0f;
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        // size
        float currentSize = sizeOverTime.Evaluate(timePassed);
        transform.localScale = new Vector2(currentSize, currentSize);

        // rotation
        float currentRoatationSpeed = rotationOverTime.Evaluate(timePassed);
        transform.Rotate(new Vector3(0,0,1) * currentRoatationSpeed * Time.deltaTime);

        // lifetime
        if (timePassed > timeAlive)
        {
            timeToFadeOut += Time.deltaTime;
            Color currentColor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timeToFadeOut);
            sp.color = currentColor;

            if(sp.color == new Color(1, 1, 1, 0))
            {
                Destroy(gameObject);
            }
        }
    }
}
