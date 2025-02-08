using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHandler : MonoBehaviour
{
    private Animator anim;
    public bool isReady;

    public AudioSource sfx_hit;
    public float maxHitPitch = 1f;
    public float minHitPitch = 0.7f;

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

        sfx_hit.pitch = Random.Range(minHitPitch, maxHitPitch);
        sfx_hit.Play();
    }

    public void WhackAnimEnded()
    {
        isReady = true;
        anim.ResetTrigger("whack");
    }
}
