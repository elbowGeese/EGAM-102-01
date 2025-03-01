using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public PlayerController player;
    public Animator anim;
    public AudioSource caughtSFX;

    public enum PedestrianStates
    {
        WALKING,
        LOOKING_TOPRIGHT,
        LOOKING_TOPLEFT,
        LOOKING_BOTTOMRIGHT,
        LOOKING_BOTTOMLEFT
    }
    public PedestrianStates state;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
        
        AddListeners();

        SetState(PedestrianStates.WALKING);
    }

    private void Update()
    {
        switch (state)
        {
            case PedestrianStates.WALKING:
                break;
            case PedestrianStates.LOOKING_TOPRIGHT:
                if (player.playerPos == PlayerController.PlayerPosition.TOPRIGHT)
                {
                    ScoreHandler.currentScore++;
                    caughtSFX.Play();
                    SetState(PedestrianStates.WALKING);
                }
                break;
            case PedestrianStates.LOOKING_TOPLEFT:
                if (player.playerPos == PlayerController.PlayerPosition.TOPLEFT)
                {
                    ScoreHandler.currentScore++;
                    caughtSFX.Play();
                    SetState(PedestrianStates.WALKING);
                }
                break;
            case PedestrianStates.LOOKING_BOTTOMRIGHT:
                if (player.playerPos == PlayerController.PlayerPosition.BOTTOMRIGHT)
                {
                    ScoreHandler.currentScore++;
                    caughtSFX.Play();
                    SetState(PedestrianStates.WALKING);
                }
                break;
            case PedestrianStates.LOOKING_BOTTOMLEFT:
                if (player.playerPos == PlayerController.PlayerPosition.BOTTOMLEFT)
                {
                    ScoreHandler.currentScore++;
                    caughtSFX.Play();
                    SetState(PedestrianStates.WALKING);
                }
                break;
        }
    }

    #region LISTENERS

    public void AddListeners()
    {
        SceneHandler.onSceneChange += RemoveListeners;
        MissCounter.onGameEnd += PauseAnimation;
    }

    public void RemoveListeners()
    {
        SceneHandler.onSceneChange -= RemoveListeners;
        MissCounter.onGameEnd -= PauseAnimation;
    }

    #endregion

    #region ANIMATOR METHODS

    public void SetState(PedestrianStates newState)
    {
        state = newState;
    }

    public void CheckFallTopRight()
    {
        if(state == PedestrianStates.LOOKING_TOPRIGHT)
        {
            anim.SetTrigger("fallRight");
            Miss();
        }
    }

    public void CheckFallTopLeft()
    {
        if(state == PedestrianStates.LOOKING_TOPLEFT)
        {
            anim.SetTrigger("fallLeft");
            Miss();
        }
    }

    public void CheckFallBottomRight()
    {
        if(state == PedestrianStates.LOOKING_BOTTOMRIGHT)
        {
            anim.SetTrigger("fallRight");
            Miss();
        }
    }

    public void CheckFallBottomLeft()
    {
        if (state == PedestrianStates.LOOKING_BOTTOMLEFT)
        {
            anim.SetTrigger("fallLeft");
            Miss();
        }
    }

    public void DestroyFromAnim()
    {
        RemoveListeners();
        Destroy(gameObject);
    }

    #endregion

    public void Miss()
    {
        FindObjectOfType<MissCounter>().currentMisses++;
    }

    public void PauseAnimation()
    {
        Debug.Log("PAUSE ANIMATION");
        anim.speed = 0;
    }
}
