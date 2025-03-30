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

    public bool goingAway = false;

    public ParticleSystem neverHitParticlePrefab;
    public AudioSource sfxEndlessMiss;

    public float backupDestroyTime = 2f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(minIdleSpeed, maxIdleSpeed);

        ChanceFlip();
    }

    public void GoAway()
    {
        if (goingAway) { return; }

        goingAway = true;
        anim.SetTrigger("away");

        // if endless mode
        if (GameObject.FindFirstObjectByType<EndlessMoleHandler>())
        {
            GameObject.FindFirstObjectByType<EndlessMoleHandler>().MissMole();

            ParticleSystem neverHitPart = Instantiate(neverHitParticlePrefab);
            neverHitPart.transform.position = transform.position;

            sfxEndlessMiss.Play();
        }

        Destroy(gameObject, backupDestroyTime);
    }

    public void DestroyFromAnim()
    {
        Destroy(gameObject);
    }

    public void GetWhacked()
    {
        if(goingAway) { return; }

        goingAway = true;

        sfxSqueak.pitch = Random.Range(minSqueakPitch, maxSqueakPitch);
        sfxSqueak.Play();

        if (GameObject.FindObjectOfType<MoleHandler>())
        {
            GameObject.FindObjectOfType<MoleHandler>().RemoveMole();
        }
        else if (GameObject.FindObjectOfType<EndlessMoleHandler>())
        {
            GameObject.FindObjectOfType<EndlessMoleHandler>().RemoveMole();
        }
        anim.SetTrigger("whacked");

        Destroy(gameObject, backupDestroyTime);
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
