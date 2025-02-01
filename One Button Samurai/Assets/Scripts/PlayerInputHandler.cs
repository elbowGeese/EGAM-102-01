using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputHandler : MonoBehaviour
{
    // components
    private Animator anim;

    // variables
    public enum PlayerStates { IDLE, ATTACK, UNGUARDED, DEAD };
    public PlayerStates state;

    private bool paused = false;

    public float unguardedTime = 1f;
    private float unguardedTimer = 0f;

    // events
    public static event Action playerAttackEvent;

    void Start()
    {
        anim = GetComponent<Animator>();

        EnableListeners();

        ChangeStateIdle();
        PausePlayer();
    }

    void Update()
    {
        if (paused) { return; }

        if (state == PlayerStates.IDLE)
        {
            // look for input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeStateAttack();
            }
        }
        else if (state == PlayerStates.UNGUARDED)
        {
            if (unguardedTimer < unguardedTime)
            {
                unguardedTimer += Time.deltaTime;

                if(unguardedTimer >= unguardedTime)
                {
                    ChangeStateIdle();
                }
            }
        }
    }

    public void EnableListeners()
    {
        SceneHandler.onSceneChange += DisableListeners;

        PauseHandler.onPauseGame += PausePlayer;
        CountdownHandler.onCountdownDone += UnpausePlayer;

        EnemyBehaviour.onPlayerHitEnemy += PlayerHitEnemy;
        EnemyBehaviour.onPlayerMissEnemy += PlayerMissEnemy;
        EnemyBehaviour.onEnemyHitPlayer += ChangeStateDead;
    }

    public void DisableListeners()
    {
        PauseHandler.onPauseGame -= PausePlayer;
        CountdownHandler.onCountdownDone -= UnpausePlayer;

        EnemyBehaviour.onPlayerHitEnemy -= PlayerHitEnemy;
        EnemyBehaviour.onPlayerMissEnemy -= PlayerMissEnemy;
        EnemyBehaviour.onEnemyHitPlayer -= ChangeStateDead;

        SceneHandler.onSceneChange -= DisableListeners;
    }

    public void PausePlayer()
    {
        paused = true;
    }

    public void UnpausePlayer()
    {
        paused = false;
    }

    public void ChangeStateIdle()
    {
        Debug.Log("PLAYER IDLE");
        state = PlayerStates.IDLE;

        anim.ResetTrigger("attack");
        anim.ResetTrigger("block");
    }

    public void ChangeStateAttack()
    {
        Debug.Log("PLAYER ATTACK");
        state = PlayerStates.ATTACK;

        // attack event
        playerAttackEvent?.Invoke();
    }

    public void PlayerHitEnemy()
    {
        Debug.Log("PLAYER HIT ENEMY");

        ChangeStateIdle();
        anim.SetTrigger("attack");
    }

    public void PlayerMissEnemy()
    {
        Debug.Log("PLAYER MISS ENEMY");

        ChangeStateUnguarded();
        anim.SetTrigger("block");
    }

    public void ChangeStateUnguarded()
    {
        Debug.Log("PLAYER UNGUARDED");

        unguardedTimer = 0f;
        state = PlayerStates.UNGUARDED;
    }

    public void ChangeStateDead()
    {
        Debug.Log("PLAYER DEAD");
        state = PlayerStates.DEAD;

        // animation
        anim.SetBool("isDead", true);
    }

    public void PlayerDied()
    {
        // end game
        GameObject.Find("SceneHandler").GetComponent<SceneHandler>().GoToScene("LoseScene");
    }
}
