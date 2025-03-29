using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissCounter : MonoBehaviour
{
    private bool isActive = false;
    private Image missImage;
    public Color activeColor;
    public Color deactiveColor;

    public ParticleSystem missParticle;

    private void Awake()
    {
        missImage = GetComponent<Image>();
    }

    public void Activate()
    {
        isActive = true;
        missImage.color = activeColor;
        missParticle.Play();
    }

    public void Deactivate()
    {
        isActive = false;
        missImage.color = deactiveColor;
    }
}
