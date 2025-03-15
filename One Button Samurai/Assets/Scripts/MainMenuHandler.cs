using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public enum MainMenuState { MAIN, LEVELS };
    public MainMenuState state;

    public GameObject mainUI;
    public GameObject levelUI;

    void Start()
    {
        SetState(MainMenuState.MAIN);
    }

    void Update()
    {
        switch (state)
        {
            case MainMenuState.MAIN:
                MainUpdate();
                break;
            case MainMenuState.LEVELS:
                break;
            default:
                break;
        }
    }

    public void SetState(MainMenuState newState)
    {
        state = newState;

        CloseMenus();

        switch (state)
        {
            case MainMenuState.MAIN:
                mainUI.SetActive(true);
                break;
            case MainMenuState.LEVELS:
                levelUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CloseMenus()
    {
        mainUI.SetActive(false);
        levelUI.SetActive(false);
    }

    public void MainUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetState(MainMenuState.LEVELS);
        }
    }
}
