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

    public GameObject hitPartPrefab;
    public GameObject missPartPrefab;

    public bool onPlayer = false;

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
            GameObject hitPart = Instantiate(hitPartPrefab, transform);
            hitPart.transform.position = transform.position;

            RemoveMole(true);
        }
        else
        {
            GameObject missPart = Instantiate(missPartPrefab, transform);
            missPart.transform.position = transform.position;
        }
    }

    private void RemoveMole(bool whacked = false)
    {
        foreach (Transform tr in transform)
        {
            if (tr.CompareTag("Mole") == true)
            {
                Mole m = tr.GetComponent<Mole>();

                if (m.goingAway) { break; }

                if (whacked) 
                { 
                    m.GetWhacked();
                    StartCoroutine(ResetHasMole(1f));
                }
                else
                { 
                    m.GoAway(); 
                    StartCoroutine(ResetHasMole(0.2f));
                }
            }
        }
    }

    IEnumerator ResetHasMole(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        hasMole = false;
        timer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HammerHandler>())
        {
            onPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HammerHandler>())
        {
            onPlayer = false;
        }
    }
}
