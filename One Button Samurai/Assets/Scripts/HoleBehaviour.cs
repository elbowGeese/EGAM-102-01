using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoleBehaviour : MonoBehaviour
{
    private Image image;
    public TMP_Text keyText;

    public bool hasMole = false;

    public float timeToRemoveMole = 2f;
    public float timer = 0f;

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();

        hasMole = false;
        timer = 0f;
    }

    private void Update()
    {
        if (hasMole && timer < timeToRemoveMole)
        {
            timer += Time.deltaTime;

            if (timer >= timeToRemoveMole)
            {
                RemoveMole();
            }
        }
    }

    public void SetKeyText(char key)
    {
        string keyString = key.ToString().ToUpper();
        keyText.text = keyString;
    }

    public void WhackHole()
    {
        if (hasMole)
        {
            RemoveMole();
        }
    }

    private void RemoveMole()
    {
        Debug.Log("REMOVING MOLE...");

        foreach (Transform tr in transform)
        {
            Debug.Log(tr.gameObject);
            if (tr.CompareTag("Mole") == true)
            {
                Debug.Log("MOLE REMOVED");
                Destroy(tr.gameObject);

                hasMole = false;
                timer = 0f;

                return;
            }
        }

        Debug.Log("MOLE NOT FOUND");
    }
}
