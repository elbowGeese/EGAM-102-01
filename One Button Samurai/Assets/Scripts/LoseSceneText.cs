using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseSceneText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = "Passed " + VariableHolder.waveProgress + "/" + VariableHolder.waveCount + " waves.";
    }
}
