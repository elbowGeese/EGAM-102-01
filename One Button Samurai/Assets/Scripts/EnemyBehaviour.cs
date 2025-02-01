using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    // components
    private Animator anim;

    // variables
    public enum EnemyStates { IDLE, CHARGE, ATTACK, UNGUARDED, DEAD };
    public EnemyStates state;

    private bool paused = false;

    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;
    public float idleTime;

    public float chargeTime = 0.5f;

    public float attackTime = 0.2f;

    public float unguardedTime = 1f;

    public float timer = 0f;

    // event actions
    public static event Action onPlayerHitEnemy;
    public static event Action onPlayerMissEnemy;
    public static event Action onEnemyHitPlayer;

    void Start()
    {
        anim = GetComponent<Animator>();

        EnableListeners();

        ChangeStateIdle();
        PauseEnemy();
    }

    void Update()
    {
        if (paused) { return; }

        if (state == EnemyStates.IDLE)
        {
            if (timer < idleTime)
            {
                timer += Time.deltaTime;

                if (timer >= idleTime)
                {
                    ChangeStateCharge();
                }
            }
        }
        else if(state == EnemyStates.CHARGE)
        {
            if (timer < chargeTime)
            {
                timer += Time.deltaTime;

                if (timer >= chargeTime)
                {
                    ChangeStateAttack();
                }
            }
        }
        else if(state == EnemyStates.ATTACK)
        {
            if (timer < attackTime)
            {
                timer += Time.deltaTime;

                if (timer >= attackTime)
                {
                    // enemy hit player event
                    onEnemyHitPlayer?.Invoke();

                    ChangeStateIdle();
                }
            }
        }
        else if(state == EnemyStates.UNGUARDED)
        {
            if (timer < unguardedTime)
            {
                timer += Time.deltaTime;

                if (timer >= unguardedTime)
                {
                    ChangeStateIdle();
                }
            }
        }
    }

    public void EnableListeners()
    {
        SceneHandler.onSceneChange += DisableListeners;

        PauseHandler.onPauseGame += PauseEnemy;
        CountdownHandler.onCountdownDone += UnpauseEnemy;

        PlayerInputHandler.playerAttackEvent += CheckIfPlayerHit;
    }

    public void DisableListeners()
    {
        CountdownHandler.onCountdownDone -= UnpauseEnemy;
        PauseHandler.onPauseGame -= PauseEnemy;

        PlayerInputHandler.playerAttackEvent -= CheckIfPlayerHit;

        SceneHandler.onSceneChange -= DisableListeners;
    }

    public void PauseEnemy()
    {
        paused = true;
    }

    public void UnpauseEnemy()
    {
        paused = false;
    }

    public void ChangeStateIdle()
    {
        state = EnemyStates.IDLE;

        timer = 0f;
        idleTime = Random.Range(minIdleTime, maxIdleTime);

        anim.ResetTrigger("charge");
        anim.ResetTrigger("attack");
        anim.ResetTrigger("guardbroke");
        anim.ResetTrigger("block");
    }

    public void ChangeStateCharge()
    {
        state = EnemyStates.CHARGE;

        timer = 0f;

        anim.SetTrigger("charge");
    }

    public void ChangeStateAttack()
    {
        state = EnemyStates.ATTACK;

        timer = 0f;
        anim.SetTrigger("attack");
    }

    public void ChangeStateUnguarded()
    {
        state = EnemyStates.UNGUARDED;

        timer = 0f;
        anim.SetTrigger("guardbroke");
    }

    public void ChangeStateDead()
    {
        state = EnemyStates.DEAD;
        anim.SetBool("isDead", true);
    }

    public void EnemyDied()
    {
        // end game
        GameObject.Find("SceneHandler").GetComponent<SceneHandler>().GoToScene("WinScene");
    }

    public void CheckIfPlayerHit()
    {
        if(state == EnemyStates.UNGUARDED)
        {
            // player hit enemy event
            onPlayerHitEnemy?.Invoke();

            ChangeStateDead();
        }
        else if (state == EnemyStates.ATTACK)
        {
            // player hit enemy event
            onPlayerHitEnemy?.Invoke();

            ChangeStateUnguarded();
        }
        else
        {
            // player miss enemy event
            onPlayerMissEnemy?.Invoke();

            ChangeStateIdle();

            // block animation
            anim.SetTrigger("block");
        }
    }
}
