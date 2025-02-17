using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    // states
    public enum CatStates { IDLE = 0, ROAMING = 1, JUMPING = 2, ADVANCING = 3, STEALING = 4, RUNAWAY = 5, PICKED = 6, FALLING = 7, STARSTRUCK = 8 }
    public CatStates state;

    // cat color
    private int color;

    // speed and distance
    public float roamingSpeed = 5f;
    public float jumpingSpeed = 10f;
    public float runawaySpeed = 10f;
    public float pickedSpeed = 7f;
    public AnimationCurve launchSpeed;
    private float moveSpeed;
    public float distanceFromTargetRadius = 1f;

    public float targetX;

    public float pickedOffset = 1f;

    // components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private AudioSource audioSource;

    // timers
    public float maxTimeToIdle = 2f;
    public float minTimeToIdle = 0.5f;
    private float idleTimer;

    public float timeToSteal = 0.5f;
    private float stealingTimer;

    public float waitTimeToRunAfterSteal = 0.8f;

    // food
    private Transform foodPosition;

    // sounds
    public AudioClip mrrClip;
    public AudioClip meowClip;
    public float maxPitch = 1.1f;
    public float minPitch = 0.9f;

    void Start()
    {
        color = Random.Range(0, 5);

        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        anim.SetFloat("color", color / 4f);
        sprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        foodPosition = GameObject.FindWithTag("Food").transform;

        audioSource = GetComponent<AudioSource>();

        SetState(CatStates.ROAMING);

        AddListeners();
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
            case CatStates.STARSTRUCK:
                IdleUpdate();
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

        anim.ResetTrigger("idle");
        anim.ResetTrigger("walk");
        anim.ResetTrigger("jump");
        anim.ResetTrigger("advance");
        anim.ResetTrigger("run");
        anim.ResetTrigger("picked");
        anim.ResetTrigger("flung");

        switch (state)
        {
            case CatStates.IDLE:
                moveSpeed = 0f;
                idleTimer = Random.Range(minTimeToIdle, maxTimeToIdle);
                anim.SetTrigger("idle");
                break;
            case CatStates.ROAMING:
                moveSpeed = roamingSpeed;
                // pick random target x
                targetX = Random.Range(VariableStorage.screenMinX, VariableStorage.screenMaxX);
                anim.SetTrigger("walk");
                break;
            case CatStates.JUMPING:
                moveSpeed = jumpingSpeed;
                anim.SetTrigger("jump");
                break;
            case CatStates.ADVANCING:
                moveSpeed = roamingSpeed;
                anim.SetTrigger("advance");
                break;
            case CatStates.STEALING:
                moveSpeed = 0f;
                stealingTimer = timeToSteal;
                anim.SetTrigger("idle");
                break;
            case CatStates.RUNAWAY:
                moveSpeed = runawaySpeed;
                MoveToRandomDirection();
                anim.SetTrigger("run");
                break;
            case CatStates.PICKED:
                audioSource.clip = mrrClip;
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.Play();

                moveSpeed = pickedSpeed;
                anim.SetTrigger("picked");
                break;
            case CatStates.FALLING:
                audioSource.clip = meowClip;
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.Play();

                moveSpeed = 0f;
                rb.gravityScale = 1f;
                LaunchTowardsMousePos();
                anim.SetTrigger("flung");
                break;
            case CatStates.STARSTRUCK:
                moveSpeed = 0f;
                idleTimer = 300f;
                anim.SetTrigger("idle");
                break;
            default:
                Debug.Log("Cannot set state because it does not exist");
                break;
        }
    }

    private void IdleUpdate()
    {
        // no moving
        if(idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;

            if(idleTimer <= 0f)
            {
                int[] weight = new int[4] { 1, 3, 3, 4 };
                if (transform.position.y == VariableStorage.counterY) { weight[2] = 0; }

                SetWeightedRandomState(weight);
            }
        }
    }

    private void RoamingUpdate()
    {
        // picked random target x and go there
        MoveToTarget(new Vector3(targetX, transform.position.y, transform.position.z));

        // if close enough to the target then change state
        if (transform.position.x <= targetX + distanceFromTargetRadius && transform.position.x >= targetX - distanceFromTargetRadius)
        {
            int[] weight = new int[4] { 5, 3, 3, 2 };
            if (transform.position.y == VariableStorage.counterY) { weight[2] = 0; }

            SetWeightedRandomState(weight);
        }
    }

    private void JumpingUpdate()
    {
        // move y to counter y
        MoveToTarget(new Vector3(transform.position.x, VariableStorage.counterY, transform.position.z));

        if (transform.position.y >= VariableStorage.counterY)
        {
            transform.position = new Vector3(transform.position.x, VariableStorage.counterY, transform.position.z);

            int[] weight = new int[4] { 5, 3, 0, 3 };
            if (transform.position.y == VariableStorage.counterY) { weight[2] = 0; }

            SetWeightedRandomState(weight);
        }
    }

    private void AdvancingUpdate()
    {
        // move towards food
        MoveToTarget(new Vector3(foodPosition.position.x, transform.position.y, transform.position.z));

        // if not on the counter, choose aonther state
        if (transform.position.y != VariableStorage.counterY)
        {
            if (transform.position.x <= foodPosition.position.x + distanceFromTargetRadius && transform.position.x >= foodPosition.position.x - distanceFromTargetRadius)
            {
                int[] weight = new int[4] { 3, 3, 10, 0 };
                SetWeightedRandomState(weight);
            }
        }

        // if close enough to the target then change state
        if (transform.position.x <= targetX + distanceFromTargetRadius && transform.position.x >= targetX - distanceFromTargetRadius)
        {
            SetState(CatStates.IDLE);
        }
    }

    private void StealingUpdate()
    {
        // if close enough to food, steal it
        if (stealingTimer > 0f)
        {
            stealingTimer -= Time.deltaTime;

            if (stealingTimer <= 0f)
            {
                if (!foodPosition.gameObject.GetComponent<FoodBehaviour>().stolen)
                {
                    foodPosition.gameObject.GetComponent<FoodBehaviour>().StealFood(transform);
                    SetState(CatStates.RUNAWAY);
                }
            }
        }
    }

    private void RunawayUpdate()
    {
        
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
        if(transform.position.y <= VariableStorage.groundY)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, VariableStorage.groundY, transform.position.z);
            SetState(CatStates.IDLE);
        }
    }

    #endregion

    #region Utility

    private void MoveToTarget(Vector3 target)
    {
        Vector3 toTargetDelta = target - transform.position;
        Vector3 toTargetDirection = toTargetDelta.normalized;

        transform.position += toTargetDirection * moveSpeed * Time.deltaTime;

        // sprite flip
        if (toTargetDirection.x > 0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }

    private void MoveToRandomDirection()
    {
        Vector2 dir = (Vector2)foodPosition.position - Vector2.zero;
        dir.Normalize();

        rb.AddForce(new Vector2(dir.x, 0f) * moveSpeed, ForceMode2D.Impulse);

        // sprite flip
        if (dir.x > 0f) { sprite.flipX = true; }
        else {  sprite.flipX = false; }
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

        // sprite flip
        if (targetDirection.x > 0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }

    private void SetWeightedRandomState(int[] weight)
    {
        // creates a weighted list
        List<int> weightedStates = new List<int>();

        // for each state
        for(int i = 0; i < weight.Length; i++)
        {
            Debug.Log(i);
            // for each weight of the state
            for (int j = 0; j <= weight[i]; j++)
            {
                // add the state
                weightedStates.Add(i);
                
            }
        }

        // chooses state from weighted list
        int randState = weightedStates[Random.Range(0, weightedStates.Count)];

        // sets state
        SetState((CatStates)randState);
    }

    #endregion

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(state == CatStates.ADVANCING)
        {
            FoodBehaviour food = collision.gameObject.GetComponent<FoodBehaviour>();
            if (food)
            {
                if (!food.stolen)
                {
                    SetState(CatStates.STEALING);
                }
            }
        }
    }

    #endregion

    #region Listeners

    public void AddListeners()
    {
        SceneHandler.onSceneChange += RemoveListeners;
        FoodBehaviour.onCatSteal += OnCatSteal;
        GameTimer.onFinishedCooking += OnFinishedCooking;
    }

    public void RemoveListeners()
    {
        SceneHandler.onSceneChange -= RemoveListeners;
        FoodBehaviour.onCatSteal -= OnCatSteal;
        GameTimer.onFinishedCooking -= OnFinishedCooking;
    }

    public void OnCatSteal()
    {
        // ignore cat that stole
        if (state == CatStates.RUNAWAY) { return; }

        StartCoroutine(WaitToRun());
    }

    IEnumerator WaitToRun()
    {
        yield return new WaitForSeconds(waitTimeToRunAfterSteal);

        SetState(CatStates.RUNAWAY);
    }

    public void OnFinishedCooking()
    {
        SetState(CatStates.STARSTRUCK);
    }

    #endregion
}
