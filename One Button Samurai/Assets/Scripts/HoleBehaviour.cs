using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoleBehaviour : MonoBehaviour
{
    private Image image;
    public TMP_Text keyText;

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetKeyText(char key)
    {
        string keyString = key.ToString().ToUpper();
        keyText.text = keyString;
    }

    public void WhackHole()
    {
        image.color = Color.red;
    }
}
