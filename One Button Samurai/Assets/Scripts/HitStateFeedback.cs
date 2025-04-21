using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitStateFeedback : MonoBehaviour
{
    private Animator anim;
    public TMP_Text label;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowFeedback(string message)
    {
        label.text = message;
        anim.SetTrigger("show");
    }
}
