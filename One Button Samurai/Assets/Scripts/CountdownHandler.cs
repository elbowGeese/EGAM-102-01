using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountdownHandler : MonoBehaviour
{
    public static event Action onCountdownDone;

    public Animator anim;

    public void ShowCountdown()
    {
        anim.SetTrigger("count");
    }

    // called at the end of the countdown animation
    public void CountdownFinished()
    {

        anim.ResetTrigger("count");
        onCountdownDone?.Invoke();
    }
}
