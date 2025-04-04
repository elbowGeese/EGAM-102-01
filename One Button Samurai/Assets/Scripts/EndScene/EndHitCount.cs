using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndHitCount : MonoBehaviour
{
    public TMP_Text lastCounter;
    public TMP_Text bestCounter;

    void Start()
    {
        lastCounter.text = "YOU HIT " + PlayerPrefs.GetInt("lastHits") + " BASEBALLS!";
        bestCounter.text = "BEST: " + PlayerPrefs.GetInt("bestHits");
    }
}
