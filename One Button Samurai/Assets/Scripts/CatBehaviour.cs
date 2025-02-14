using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    public enum CatStates { IDLE = 0, ROAMING = 1, JUMPING = 2, ADVANCING = 3, STEALING = 4, RUNAWAY = 5, PICKED = 6, FALLING = 7 }
    public CatStates state;

    public float roamingSpeed = 5f;
    public float jumpingSpeed = 10f;
    public float runawaySpeed = 10f;
    public float pickedSpeed = 7f;
    public AnimationCurve launchSpeed;
    private float moveSpeed;
    public float distanceFromTargetRadius = 1f;

    public float targetX;

    public float pickedOffset = 1f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetState(CatStates.ROAMING);
    }

    void Update()
    {
        switch (state)
        {
            case CatStates.IDLE:
                IdleUpdate();
                break;
            case CatStates.ROAMING:
                RoamingUpdate();
                break;
            case CatStates.JUMPING:
                JumpingUpdate();
                break;
            case CatStates.ADVANCING:
                AdvancingUpdate();
                break;
            case CatStates.STEALING:
                StealingUpdate();
                break;
            case CatStates.RUNAWAY:
                RunawayUpdate();
                break;
            case CatStates.PICKED:
                PickedUpdate();
                break;
            case CatStates.FALLING:
                FallingUpdate();
                break;
            default:
                Debug.Log("NO STATE FOUND");
                break;
        }
    }

    #region State Behaviour

    public void SetState(CatStates newState)
    {
        state = newState;

        switch (state)
        {
            case CatStates.IDLE:
                moveSpeed = 0f;
                break;
            case CatStates.ROAMING:
                moveSpeed = roamingSpeed;

                // pick random target x
                targetX = Random.Range(VariableStorage.screenMinX, VariableStorage.screenMaxX);

                break;
            case CatStates.JUMPING:
                moveSpeed = jumpingSpeed;
                break;
            case CatStates.ADVANCING:
                moveSpeed = roamingSpeed;
                break;
            case CatStates.STEALING:
                moveSpeed = 0f;
                break;
            case CatStates.RUNAWAY:
                moveSpeed = runawaySpeed;
                break;
            case CatStates.PICKED:
                moveSpeed = pickedSpeed;
                break;
            case CatStates.FALLING:
                moveSpeed = 0f;
                rb.gravityScale = 1f;
                LaunchTowardsMousePos();
                break;
            default:
                Debug.Log("Cannot set state because it does not exist");
                break;
        }
    }

    private void IdleUpdate()
    {
        // no moving
    }

    private void RoamingUpdate()
    {
        // picked random target x and go there
        MoveToTarget(new Vector3(targetX, transform.position.y, transform.position.z));

        // if close enough to the target then change state
        if (transform.position.x <= targetX + distanceFromTargetRadius && transform.position.x >= targetX - distanceFromTargetRadius)
        {
            int[] weight = new int[6] { 5, 3, 1, 2, 0, 0 };
            if (transform.position.y == VariableStorage.counterY) { weight[2] = 0; }

            SetWeightedRandomState(weight);
        }
    }

    private void JumpingUpdate()
    {
        // move y to counter y
    }

    private void AdvancingUpdate()
    {
        // move towards food
    }

    private void StealingUpdate()
    {
        // if close enough to food, steal it
    }

    private void RunawayUpdate()
    {
        // once cat has food, run away!
    }

    private void PickedUpdate()
    {
        // follow mouse
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 targetPos = new Vector3(worldPos.x, worldPos.y - pickedOffset, transform.position.z);

        if (transform.position.x >= targetPos.x - distanceFromTargetRadius && transform.position.x <= targetPos.x + distanceFromTargetRadius)
        {
            if (transform.position.y >= targetPos.y - distanceFromTargetRadius && transform.position.y <= targetPos.y + distanceFromTargetRadius)
            {
                return;
            }
        }

        MoveToTarget(targetPos);
    }

    private void FallingUpdate()
    {
        // back to floor
        
    }

    #endregion

    #region Utility

    private void MoveToTarget(Vector3 target)
    {
        Vector3 toTargetDelta = target - transform.position;
        Vector3 toTargetDirection = toTargetDelta.normalized;

        transform.position += toTargetDirection * moveSpeed * Time.deltaTime;
    }

    private void LaunchTowardsMousePos()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 targetPos = new Vector3(worldPos.x, worldPos.y - pickedOffset, transform.position.z);

        Vector3 toTargetDelta = targetPos - transform.position;
        Vector2 targetDirection = (Vector2)toTargetDelta.normalized;

        float speed = launchSpeed.Evaluate(Vector2.Distance(targetPos, transform.position));
        
        rb.AddForce(targetDirection * speed, ForceMode2D.Impulse);
    }

    private void SetWeightedRandomState(int[] weight)
    {
        // creates a weighted list
        List<int> weightedStates = new List<int>();

        foreach (int weightIndex in weight) 
        {
            for(int i = 0; i < weight[weightIndex]; i++)
            {
                weightedStates.Add(weightIndex);
            }
        }

        // chooses state from weighted list
        int randState = weightedStates[Random.Range(0, weightedStates.Count)];

        // sets state
        SetState((CatStates)randState);
    }

    #endregion

}
