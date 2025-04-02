using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    enum BaseballState { TRAVELLING, HITWINDOW, HIT, MISS }
    private BaseballState state;

    private SpriteRenderer sp;

    // travelling
    public AnimationCurve sizeOverTime;
    public AnimationCurve rotationOverTime;

    private float timePassed = 0f;
    public float timeAlive = 4f;

    // hit window
    public float swingWindow = 1f;
    private float windowTime = 0f;
    [Range(0f,1f)]
    public float minSwingSpeed = 0.5f;

    // miss
    private float timeToFadeOut = 0f;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        transform.Rotate(new Vector3(0, 0, 1) * Random.Range(0, 360));

        SetState(BaseballState.TRAVELLING);
    }

    void Update()
    {
        switch (state)
        {
            case BaseballState.TRAVELLING:
                TravellingUpdate(); break;
            case BaseballState.HITWINDOW:
                HitWindowUpdate(); break;
            case BaseballState.HIT:
                HitUpdate(); break;
            case BaseballState.MISS:
                MissUpdate(); break;
            default:
                break;
        }
    }

    private void SetState(BaseballState newState)
    {
        state = newState;
    }

    void TravellingUpdate()
    {
        timePassed += Time.deltaTime;

        // size
        float currentSize = sizeOverTime.Evaluate(timePassed);
        transform.localScale = new Vector2(currentSize, currentSize);

        // rotation
        float currentRoatationSpeed = rotationOverTime.Evaluate(timePassed);
        transform.Rotate(new Vector3(0, 0, 1) * currentRoatationSpeed * Time.deltaTime);

        // lifetime
        if (timePassed > timeAlive)
        {
            SetState(BaseballState.HITWINDOW);
        }
    }

    void HitWindowUpdate()
    {
        windowTime += Time.deltaTime;

        // if player swings in time
        if (FindAnyObjectByType<BatterSwing>().IsAtMaxSwing(out float battingSpeed))
        {
            Debug.Log(battingSpeed);
            if(battingSpeed >= minSwingSpeed) { SetState(BaseballState.HIT); }
        }

        // window open whole time
        if(windowTime >= swingWindow)
        {
            SetState(BaseballState.MISS);
        }
    }

    void HitUpdate()
    {
        Debug.Log("HIT!");
        Destroy(gameObject);
    }

    void MissUpdate()
    {
        timeToFadeOut += Time.deltaTime;

        Color currentColor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), timeToFadeOut);
        sp.color = currentColor;

        if (sp.color == new Color(1, 1, 1, 0))
        {
            Destroy(gameObject);
        }
    }

}
