using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHandler : MonoBehaviour
{
    private Animator anim;
    public bool isReady;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isReady = true;
    }

    public void Whack(Vector2 moveTo)
    {
        isReady = false;
        anim.SetTrigger("whack");
        transform.position = moveTo;
    }

    public void WhackAnimEnded()
    {
        isReady = true;
        anim.ResetTrigger("whack");
    }
}
