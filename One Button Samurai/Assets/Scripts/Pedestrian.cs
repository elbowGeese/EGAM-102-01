using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public PlayerController player;
    public Animator anim;

    public bool hasWalkedBottom = false;
    public bool hasWalkedTop = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }

    public void EndWalkingBottom()
    {
        hasWalkedBottom = true;

        if (hasWalkedTop)
        {
            // end pedestrian
            Destroy(gameObject);
        }
        else
        {
            anim.SetBool("walkingTop", true);
        }
    }

    public void EndWalkingTop()
    {
        hasWalkedTop = true;

        if (hasWalkedBottom)
        {
            // end pedestrian
            Destroy(gameObject);
        }
        else
        {
            anim.SetBool("walkingTop", false);
        }
    }

    public void CheckFallTopRight()
    {
        if(player.playerPos == PlayerController.PlayerPosition.TOPRIGHT)
        {
            return;
        }
        else
        {
            anim.SetTrigger("fallRight");
            // MISS
        }
    }

    public void CheckFallTopLeft()
    {
        if (player.playerPos == PlayerController.PlayerPosition.TOPLEFT)
        {
            return;
        }
        else
        {
            anim.SetTrigger("fallLeft");
            // MISS
        }
    }

    public void CheckFallBottomRight()
    {
        if (player.playerPos == PlayerController.PlayerPosition.BOTTOMRIGHT)
        {
            return;
        }
        else
        {
            anim.SetTrigger("fallRight");
            // MISS
        }
    }

    public void CheckFallBottomLeft()
    {
        if (player.playerPos == PlayerController.PlayerPosition.BOTTOMLEFT)
        {
            return;
        }
        else
        {
            anim.SetTrigger("fallLeft");
            // MISS
        }
    }
}
