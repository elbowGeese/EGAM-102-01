using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Goal : MonoBehaviour
{
    public float timeBetweenRbFall = 0.2f;
    public float completeTime = 3f;
    public Coroutine timer;

    public SpriteRenderer fill;
    private Color fillColor;

    public enum GoalState { ON, OFF, COMPLETE };
    public GoalState state;

    public float onScale = 1f;
    public float offScale = 0.5f;

    void Start()
    {
        fillColor = fill.color;
        SetState(GoalState.OFF);
    }

    public void SetState(GoalState newState)
    {
        state = newState;

        switch (state)
        {
            case GoalState.ON:
                TurnOn();
                break;
            case GoalState.OFF:
                TurnOff();
                break;
            case GoalState.COMPLETE:
                StartCoroutine(CompleteLevel());
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && state == GoalState.OFF)
        {
            SetState(GoalState.ON);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && state == GoalState.ON)
        {
            SetState(GoalState.OFF);
        }
    }

    void TurnOn()
    {
        fill.color = Color.white;
        transform.localScale = new Vector3(onScale, onScale, onScale);

        timer = StartCoroutine(CountdownTimer());
    }

    void TurnOff()
    {
        fill.color = fillColor;
        transform.localScale = new Vector3 (offScale, offScale, offScale);

        if(timer != null)
        {
            StopCoroutine(timer);
        }
    }

    IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(completeTime);

        SetState(GoalState.COMPLETE);
    }

    IEnumerator CompleteLevel()
    {
        Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();

        foreach(Rigidbody2D r in rbs)
        {
            if(r.bodyType == RigidbodyType2D.Static)
            {
                r.bodyType = RigidbodyType2D.Dynamic;
                yield return new WaitForSeconds(timeBetweenRbFall);
            }
        }

        yield return new WaitForSeconds(timeBetweenRbFall * 5);

        FindObjectOfType<SceneHandler>().GoToNextLevel();
    }
}
