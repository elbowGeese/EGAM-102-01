using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private Animator anim;
    public float maxIdleSpeed = 2f;
    public float minIdleSpeed = 1f;

    public AudioSource sfxSqueak;
    public float maxSqueakPitch = 1.1f;
    public float minSqueakPitch = 0.5f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(minIdleSpeed, maxIdleSpeed);

        ChanceFlip();
    }

    public void GoAway()
    {
        anim.SetTrigger("away");
    }

    public void DestroyFromAnim()
    {
        Destroy(gameObject);
    }

    public void GetWhacked()
    {
        sfxSqueak.pitch = Random.Range(minSqueakPitch, maxSqueakPitch);
        sfxSqueak.Play();

        GameObject.FindObjectOfType<MoleHandler>().RemoveMole();
        anim.SetTrigger("whacked");
    }

    public void ChanceFlip()
    {
        bool flip = Random.Range(0, 2) == 1;

        if (flip)
        {
            Vector3 flippedScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            transform.localScale = flippedScale;
        }
    }
}
