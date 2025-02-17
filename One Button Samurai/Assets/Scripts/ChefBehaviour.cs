using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ChefBehaviour : MonoBehaviour
{
    public enum ChefStates { IDLE, MOVING, END }
    public ChefStates state;

    private Animator anim;
    private AudioSource audioSource;

    public float maxIdleTime = 7f;
    public float minIdleTime = 2f;
    private float idleTimer;

    private float targetX;
    public float moveSpeed = 1f;

    public AudioClip winClip;
    public AudioClip loseClip;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        SetState(ChefStates.IDLE);

        AddListeners();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case ChefStates.IDLE:
                IdleUpdate();
                break;
            case ChefStates.MOVING:
                MovingUpdate();
                break;
            case ChefStates.END:
                break;
            default:
                Debug.Log("State does not exist.");
                break;
        }
    }

    public void SetState(ChefStates newState)
    {
        state = newState;

        switch (state)
        {
            case ChefStates.IDLE:
                idleTimer = Random.Range(minIdleTime, maxIdleTime);
                break;
            case ChefStates.MOVING:
                targetX = Random.Range(VariableStorage.screenMinX + 2f, VariableStorage.screenMaxX - 2f);
                break;
            case ChefStates.END:
                break;
            default:
                Debug.Log("Cannot set state.");
                break;
        }
    }

    private void IdleUpdate()
    {
        if(idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;

            if(idleTimer <= 0f)
            {
                SetState(ChefStates.MOVING);
            }
        }
    }

    private void MovingUpdate()
    {
        MoveToTarget(new Vector3(targetX, transform.position.y, transform.position.z));

        if(transform.position.x >= targetX - 0.1f && transform.position.x <= targetX + 0.1f)
        {
            SetState(ChefStates.IDLE);
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        Vector3 toTargetDelta = target - transform.position;
        Vector3 toTargetDirection = toTargetDelta.normalized;

        transform.position += toTargetDirection * moveSpeed * Time.deltaTime;
    }

    public void AddListeners()
    {
        SceneHandler.onSceneChange += RemoveListeners;
        FoodBehaviour.onCatSteal += Lose;
        GameTimer.onFinishedCooking += Win;
    }

    public void RemoveListeners()
    {
        SceneHandler.onSceneChange -= RemoveListeners;
        FoodBehaviour.onCatSteal -= Lose;
        GameTimer.onFinishedCooking -= Win;
    }

    public void Win()
    {
        anim.SetTrigger("win");

        audioSource.clip = winClip;
        audioSource.Play();

        SetState(ChefStates.END);
    }

    public void Lose()
    {
        anim.SetTrigger("lose");

        audioSource.clip = loseClip;
        audioSource.Play();

        SetState(ChefStates.END);
    }
}
