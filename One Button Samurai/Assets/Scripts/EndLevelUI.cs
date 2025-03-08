using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLevelUI : MonoBehaviour
{
    public GameObject endLevelUI;
    public TMP_Text bounceScoreText;

    void Start()
    {
        endLevelUI.SetActive(false);
    }

    public void EndLevel()
    {
        endLevelUI.SetActive(true);
        bounceScoreText.text = "It took " + gameObject.GetComponent<Score>()._score.ToString() + " bounces.";
    }
}
