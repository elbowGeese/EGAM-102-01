using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    public enum BaseballState { TRAVELLING, HITWINDOW, HIT, MISS }
    public BaseballState state;

    private SpriteRenderer sp;

    // positioning guide
    public Transform positioningGuide;
    public float guideStartScale = 1f;
    public float guideEndScale = 0.18f;
    public Color guideStartColor;
    public Color guideMiddleColor;
    public Color guideEndColor;
    [Range(0f, 1f)]
    public float percentToGreen = 0.8f;

    // travelling
    public AnimationCurve sizeOverTime;
    public AnimationCurve rotationOverTime;
    private Vector2 initPosition;
    private Vector2 targetPosition;
    public float targetPosOffset = 1f;

    private float timePassed = 0f;
    public float timeAlive = 4f;

    // hit window
    public float swingWindow = 1f;
    private float windowTime = 0f;
    [Range(0f,1f)]
    public float minSwingSpeed = 0.5f;

    // hit
    private Vector2 hitInitPos;
    private Vector2 hitTargetPos;
    private float hitTimePassed = 0f;
    public float hitTimeAlive = 0.5f;
    private float hitMinY = 4f;
    public float hitTargetPosOffsetX = 15f;
    public float hitTargetPosOffsetY = 4f;
    private Vector2 hitInitSize;

    // miss
    private float timeToFadeOut = 0f;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        transform.Rotate(new Vector3(0, 0, 1) * Random.Range(0, 360));

        positioningGuide.gameObject.GetComponent<SpriteRenderer>().color = guideStartColor;
        positioningGuide.localScale = new Vector2(guideStartScale, guideStartScale);

        SetState(BaseballState.TRAVELLING);
    }

    void Update()
    {
        // paused
        if (PauseHandler.instance.pauseMenu.activeSelf) { return; }

        // unpaused
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

        switch (state)
        {
            case BaseballState.TRAVELLING:
                initPosition = transform.position;
                Vector2 aimboxPos = GameObject.FindWithTag("AimBox").transform.position;
                targetPosition = new Vector2(aimboxPos.x + Random.Range(-targetPosOffset, targetPosOffset), aimboxPos.y + Random.Range(-targetPosOffset, targetPosOffset));
                break;
            case BaseballState.HITWINDOW:
                positioningGuide.gameObject.GetComponent<SpriteRenderer>().color = guideEndColor;
                break;
            case BaseballState.HIT:
                FindFirstObjectByType<HitCounter>().AddHit();

                hitInitSize = transform.localScale;
                hitInitPos = transform.position;
                hitTargetPos = new Vector2(Random.Range(-hitTargetPosOffsetX, hitTargetPosOffsetX), hitMinY + Random.Range(0f, hitTargetPosOffsetY));

                break;
            case BaseballState.MISS:
                FindFirstObjectByType<HitCounter>().ResetStreak();
                break;
            default:
                break;
        }
    }

    void TravellingUpdate()
    {
        timePassed += Time.deltaTime;

        // size
        float currentSize = sizeOverTime.Evaluate(timePassed / timeAlive);
        transform.localScale = new Vector2(currentSize, currentSize);
        UpdatePositioningGuide(timePassed);

        // rotation
        float currentRoatationSpeed = rotationOverTime.Evaluate(timePassed / timeAlive);
        transform.Rotate(new Vector3(0, 0, 1) * currentRoatationSpeed * Time.deltaTime);

        // position
        transform.position = Vector2.Lerp(initPosition, targetPosition, timePassed / timeAlive);

        // lifetime
        if (timePassed > timeAlive)
        {
            SetState(BaseballState.HITWINDOW);
        }
    }

    private void UpdatePositioningGuide(float timePassed)
    {
        // size
        positioningGuide.localScale = Vector2.Lerp(new Vector2(guideStartScale, guideStartScale), new Vector2(guideEndScale, guideEndScale), timePassed / timeAlive);

        // color
        if (timePassed <= timeAlive * percentToGreen)
        {
            positioningGuide.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(guideStartColor, guideMiddleColor, timePassed / (timeAlive * percentToGreen));
        }
        else
        {
            positioningGuide.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(guideMiddleColor, guideEndColor, (timePassed - (timeAlive * percentToGreen)) / (timeAlive * percentToGreen));
        }
    }

    void HitWindowUpdate()
    {
        windowTime += Time.deltaTime;

        // if player swings in time
        if (FindAnyObjectByType<BatterSwing>().IsAtMaxSwing(out float battingSpeed))
        {
            if(battingSpeed >= minSwingSpeed) 
            {
                positioningGuide.localScale = Vector3.zero;
                SetState(BaseballState.HIT); 
            }
        }

        // window open whole time
        if(windowTime >= swingWindow)
        {
            positioningGuide.localScale = Vector3.zero;
            SetState(BaseballState.MISS);
        }
    }

    void HitUpdate()
    {
        hitTimePassed += Time.deltaTime;

        // size
        transform.localScale = Vector2.Lerp(hitInitSize, Vector2.zero, hitTimePassed / hitTimeAlive);

        // rotation

        // position
        transform.position = Vector2.Lerp(hitInitPos, hitTargetPos, hitTimePassed / hitTimeAlive);

        if(hitTimePassed > hitTimeAlive)
        {
            Destroy(gameObject);
        }
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
