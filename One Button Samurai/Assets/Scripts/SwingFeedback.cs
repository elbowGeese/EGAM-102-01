using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingFeedback : MonoBehaviour
{
    public enum SwingFeedbackType { TooEarly, TooLate, TooSlow, Perfect }

    BatterSwing bat;
    private bool isSwung = false;

    public Transform feedbackPosition;
    public GameObject tooEarlyPrefab;
    public GameObject tooLatePrefab;
    public GameObject tooSlowPrefab;
    public GameObject perfectPrefab;

    private void Start()
    {
        bat = GetComponent<BatterSwing>();
    }

    void Update()
    {
        // paused
        if (PauseHandler.instance.pauseMenu.activeSelf) { return; }

        // unpaused
        Baseball ball = FindAnyObjectByType<Baseball>();
        if (ball != null)
        {
            if (!isSwung && bat.IsAtMaxSwing(out float battingSpeed))
            {
                isSwung = true;

                // check for ball state
                switch (ball.state)
                {
                    case Baseball.BaseballState.TRAVELLING:
                        SpawnSwingFeedback(SwingFeedbackType.TooEarly);
                        break;
                    case Baseball.BaseballState.MISS:
                        SpawnSwingFeedback(SwingFeedbackType.TooLate);
                        break;
                    case Baseball.BaseballState.HITWINDOW:
                        if (battingSpeed < ball.minSwingSpeed)
                        {
                            SpawnSwingFeedback(SwingFeedbackType.TooSlow);
                        }
                        break;
                    default:
                        Debug.Log("No corresponding swing feedback type.");
                        break;
                }
            }
            else if(isSwung && !bat.IsAtMaxSwing(out float battingSpeed2))
            {
                // reset swing
                isSwung = false;
            }
        }
    }

    public void SpawnSwingFeedback(SwingFeedbackType feedbackType)
    {
        // spawn the corresponding feedback
        switch (feedbackType)
        {
            case SwingFeedbackType.TooEarly:
                Instantiate(tooEarlyPrefab, feedbackPosition);
                break;
            case SwingFeedbackType.TooLate:
                Instantiate(tooLatePrefab, feedbackPosition);
                break;
            case SwingFeedbackType.TooSlow:
                Instantiate(tooSlowPrefab, feedbackPosition);
                break;
            case SwingFeedbackType.Perfect:
                Instantiate(perfectPrefab, feedbackPosition);
                break;
            default:
                Debug.Log("No corresponding swing feedback prefab to spawn.");
                break;
        }
    }
}
