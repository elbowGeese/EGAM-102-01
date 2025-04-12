using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallTypeDisplay : MonoBehaviour
{
    private Animator anim;
    private TMP_Text label;

    void Start()
    {
        anim = GetComponent<Animator>();
        label = GetComponent<TMP_Text>();
    }

    public void Show(string text)
    {
        label.text = text + "!";
        anim.SetTrigger("show");
    }
}
