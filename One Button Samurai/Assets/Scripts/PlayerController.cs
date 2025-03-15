using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { OPEN, CLOSE };
    public PlayerState state;

    public HingeJoint2D hinge;
    public float motorStrength = 100f;
    public float maxMotorTorque = 1000f;

    void Start()
    {
        SetState(PlayerState.OPEN);
    }

    void Update()
    {
        switch (state)
        {
            case PlayerState.OPEN:
                OpenUpdate();
                break;
            case PlayerState.CLOSE:
                CloseUpdate();
                break;
            default:
                break;
        }
    }

    public void SetState(PlayerState newState)
    {
        state = newState;

        switch (state)
        {
            case PlayerState.OPEN:
                SetMotorSpeed(-motorStrength);
                break;
            case PlayerState.CLOSE:
                SetMotorSpeed(motorStrength);
                break;
        }
    }

    public void SetMotorSpeed(float speed)
    {
        JointMotor2D motor = hinge.motor;

        hinge.useMotor = true;
        motor.motorSpeed = speed;
        motor.maxMotorTorque = maxMotorTorque;

        hinge.motor = motor;
    }

    public void OpenUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetState(PlayerState.CLOSE);
        }
    }

    public void CloseUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SetState(PlayerState.OPEN);
        }
    }
}
