using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessEndScene : MonoBehaviour
{
    public TMP_Text endDesc;

    void Start()
    {
        endDesc.text = "Wow!\nYou whacked " + EndlessModeHitCountVariable.hitCount + " moles!";
    }

}
