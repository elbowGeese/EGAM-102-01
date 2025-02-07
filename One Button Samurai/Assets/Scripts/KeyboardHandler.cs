using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    public List<char> chars;
    public List<HoleBehaviour> holes;

    public Dictionary<char, HoleBehaviour> keyValuePairs = new Dictionary<char, HoleBehaviour>();

    private HammerHandler hammer;

    void Start()
    {
        // dictionaries cant be exposed to the editor i hate it here
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].SetKeyText(chars[i]);
            keyValuePairs.Add(chars[i], holes[i]);
        }

        hammer = GameObject.FindObjectOfType<HammerHandler>();
    }

    void Update()
    {
        // wait for animation to finish before allowing key press
        if (!hammer.isReady) { return; }

        // get key input, no mouse
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            // store input as the first char pressed
            string _input = Input.inputString.ToLower();
            if (_input == null || _input == "") { return; } // catch non-string keys

            char key = _input[0];

            // check if dictionary has they char
            if (keyValuePairs.ContainsKey(key))
            {
                // whack the hole associated with the key
                keyValuePairs[key].WhackHole();
                hammer.Whack(keyValuePairs[key].transform.position);
            }
        }
    }
}
