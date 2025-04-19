using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baseball : MonoBehaviour
{
    [Header("Main")]
    public ThrowType.PitchType pitchType;

    public enum BaseballState { TRAVELLING, HITWINDOW, HIT, MISS }
    public BaseballState state;

    public enum HitState { INSIDE, OUTSIDE, HOMERUN, GROUNDBALL }
    public HitState hitState;

    private SpriteRenderer sp;

    [Header("Positioning")]
    public Transform positioningGuide;
    public float guideStartScale = 1f;
    public float guideEndScale = 0.18f;
    public Color guideStartColor;
    public Color guideMiddleColor;
    public Color guideEndColor;
    [Range(0f, 1f)]
    public float percentToGreen = 0.8f;

    [Header("Travelling State")]
    public AnimationCurve sizeOverTime;
    public AnimationCurve rotationOverTime;
    public AnimationCurve pathCurveOverTime;
    private Vector2 initPosition;
    private Vector2 targetPosition;
    public float targetPosOffset = 1f;

    private float timePassed = 0f;
    public float timeAlive = 4f;

    [Header("Hit Window State")]
    public float swingWindow = 1f;
    private float windowTime = 0f;
    [Range(0f,1f)]
    public float minSwingSpeed = 0.5f;
    public GameObject boomParticle;

    [Header("Hit State")]
    public float hitTimeAlive = 0.5f;
    private float hitTimePassed = 0f;
    public float hitTargetPosOffsetX = 15f;
    private Vector2 hitInitSize;
    private Vector2 hitInitPos;
    private Vector2 hitTargetPos;

    [Header("Hit State : Homerun")]
    public float minHomerunY = 6f;
    public float maxHomerunY = 8f;

    [Header("Hit State : Outside")]
    public float minOutsideY = 4f;
    public float maxOutsideY = 6f;

    [Header("Hit State : Inside")]
    public float minInsideY = 0f;
    public float maxInsideY = 4f;

    [Header("Hit State : Ground Ball")]
    public float minGroundballY = -4;
    public float maxGroundballY = -2;

    [Header("Miss State")]
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

    private void SetState(BaseballState newState, HitState newHitState = HitState.HOMERUN)
    {
        state = newState;
        hitState = newHitState;

        switch (state)
        {
            case BaseballState.TRAVELLING:
                initPosition = transform.position;
                Vector2 aimboxPos = FindAimbox();
                targetPosition = new Vector2(aimboxPos.x + Random.Range(-targetPosOffset, targetPosOffset), aimboxPos.y + Random.Range(-targetPosOffset, targetPosOffset));
                break;
            case BaseballState.HITWINDOW:
                positioningGuide.gameObject.GetComponent<SpriteRenderer>().color = guideEndColor;
                break;
            case BaseballState.HIT:
                if (FindFirstObjectByType<HitCounter>()) { FindFirstObjectByType<HitCounter>().AddHit(hitState); }

                if (FindFirstObjectByType<BallTypeDisplay>()) { FindFirstObjectByType<BallTypeDisplay>().Show(pitchType.ToString()); }

                hitInitSize = transform.localScale;
                hitInitPos = transform.position;

                float randY = 0f;
                switch (hitState)
                {
                    case HitState.INSIDE:
                        randY = Random.Range(minInsideY, maxInsideY);
                        break;
                    case HitState.OUTSIDE:
                        randY = Random.Range(minOutsideY, maxOutsideY);
                        break;
                    case HitState.HOMERUN:
                        randY = Random.Range(minHomerunY, maxHomerunY);
                        break;
                    case HitState.GROUNDBALL:
                        randY = Random.Range(minGroundballY, maxGroundballY);
                        break;
                    default:
                        Debug.Log("No hit state range to target.");
                        break;
                }
                hitTargetPos = new Vector2(Random.Range(-hitTargetPosOffsetX, hitTargetPosOffsetX), randY);

                break;
            case BaseballState.MISS:
                if (pitchType != ThrowType.PitchType.SPLITTER && FindFirstObjectByType<HitCounter>()) { FindFirstObjectByType<HitCounter>().ResetStreak(); }

                if (FindFirstObjectByType<BallTypeDisplay>()) { FindFirstObjectByType<BallTypeDisplay>().Show(pitchType.ToString()); }

                break;
            default:
                break;
        }
    }

    private Vector2 FindAimbox()
    {
        GameObject[] aimBoxes = GameObject.FindGameObjectsWithTag("AimBox");

        switch (pitchType)
        {
            case ThrowType.PitchType.FASTBALL:
                foreach(GameObject aimBox in aimBoxes)
                {
                    if(aimBox.name == "MiddleAim") { return aimBox.transform.position; }
                } 
                break;
            case ThrowType.PitchType.CURVEBALL:
                foreach (GameObject aimBox in aimBoxes)
                {
                    if (aimBox.name == "OuterAim") { return aimBox.transform.position; }
                }
                break;
            case ThrowType.PitchType.SCREWBALL:
                foreach (GameObject aimBox in aimBoxes)
                {
                    if (aimBox.name == "InnerAim") { return aimBox.transform.position; }
                }
                break;
            case ThrowType.PitchType.SPLITTER:
                foreach (GameObject aimBox in aimBoxes)
                {
                    if (aimBox.name == "InnerAim") { return aimBox.transform.position; }
                }
                break;
            default:
                Debug.Log("No aimbox relegated to pitch.");
                break;
        }

        // if it is assigned wrong, just grab the first aimbox the computer found
        return aimBoxes[0].transform.position;
    }

    void TravellingUpdate()
    {
        timePassed += Time.deltaTime;

        // size
        float currentSize = sizeOverTime.Evaluate(timePassed / timeAlive);
        transform.localScale = new Vector2(currentSize, currentSize);
        UpdatePositioningGuide(timePassed);

        // rotation
        float currentRotationSpeed = rotationOverTime.Evaluate(timePassed / timeAlive);
        transform.Rotate(new Vector3(0, 0, 1) * currentRotationSpeed * Time.deltaTime);

        // position
        Vector2 newPos = Vector2.Lerp(initPosition, targetPosition, timePassed / timeAlive);
        // path curve
        transform.position = new Vector2(newPos.x + pathCurveOverTime.Evaluate(timePassed / timeAlive), newPos.y);

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

            if (battingSpeed >= minSwingSpeed) 
            {
                positioningGuide.localScale = Vector3.zero;
                if (FindFirstObjectByType<SwingFeedback>() != null)
                {
                    FindFirstObjectByType<SwingFeedback>().SpawnSwingFeedback(SwingFeedback.SwingFeedbackType.Perfect);
                }

                GameObject boom = Instantiate(boomParticle);
                boom.transform.position = this.transform.position;

                switch (pitchType)
                {
                    case ThrowType.PitchType.FASTBALL:
                        SetState(BaseballState.HIT, HitState.HOMERUN);
                        break;
                    case ThrowType.PitchType.CURVEBALL:
                        SetState(BaseballState.HIT, HitState.HOMERUN);
                        break;
                    case ThrowType.PitchType.SCREWBALL:
                        SetState(BaseballState.HIT, HitState.HOMERUN);
                        break;
                    case ThrowType.PitchType.SPLITTER:
                        SetState(BaseballState.HIT, HitState.GROUNDBALL);
                        break;
                    default:
                        Debug.Log("Unknown pitch type, unknown hit type...");
                        SetState(BaseballState.HIT);
                        break;
                }
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
