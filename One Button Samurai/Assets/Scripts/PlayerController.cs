using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerPosition { TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT }
    public PlayerPosition playerPos;

    public GameObject topLeft;
    public GameObject topRight;
    public GameObject bottomLeft;
    public GameObject bottomRight;

    void Start()
    {
        SetPosition(PlayerPosition.TOPRIGHT);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // TOP LEFT
            SetPosition(PlayerPosition.TOPLEFT);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // BOTTOM LEFT
            SetPosition(PlayerPosition.BOTTOMLEFT);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            // TOP RIGHT
            SetPosition(PlayerPosition.TOPRIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // BOTTOM RIGHT
            SetPosition(PlayerPosition.BOTTOMRIGHT);
        }
    }

    public void SetPosition(PlayerPosition pos)
    {
        playerPos = pos;

        CloseAllPositions();

        switch (playerPos)
        {
            case PlayerPosition.TOPLEFT:
                topLeft.SetActive(true);
                break;
            case PlayerPosition.TOPRIGHT:
                topRight.SetActive(true);
                break;
            case PlayerPosition.BOTTOMLEFT:
                bottomLeft.SetActive(true);
                break;
            case PlayerPosition.BOTTOMRIGHT:
                bottomRight.SetActive(true);
                break;
            default:
                Debug.Log("Player position does not exist.");
                break;
        }
    }

    public void SetTopLeft()
    {
        SetPosition(PlayerPosition.TOPLEFT);
    }

    public void SetTopRight()
    {
        SetPosition(PlayerPosition.TOPRIGHT);
    }

    public void SetBottomLeft()
    {
        SetPosition(PlayerPosition.BOTTOMLEFT);
    }

    public void SetBottomRight()
    {
        SetPosition(PlayerPosition.BOTTOMRIGHT);
    }

    public void CloseAllPositions()
    {
        topLeft.SetActive(false);
        topRight.SetActive(false);
        bottomLeft.SetActive(false);
        bottomRight.SetActive(false);
    }

}
