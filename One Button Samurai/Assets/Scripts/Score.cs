using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int _score = 0;
    public TMP_Text scoreText;

    void Update()
    {
        scoreText.text = _score.ToString();
    }
}
