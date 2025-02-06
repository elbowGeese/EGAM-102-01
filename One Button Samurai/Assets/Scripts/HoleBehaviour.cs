using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleBehaviour : MonoBehaviour
{
    private Image image;

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }

    public void WhackHole()
    {
        image.color = Color.red;
    }
}
