using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    public List<char> chars;
    public List<HoleBehaviour> holes;

    public Dictionary<char, HoleBehaviour> keyValuePairs = new Dictionary<char, HoleBehaviour>();

    void Start()
    {
        // dictionaries cant be exposed to the editor i hate it here
        for (int i = 0; i < holes.Count; i++)
        {
            keyValuePairs.Add(chars[i], holes[i]);
        }
    }

    void Update()
    {
        // get key input, no mouse
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {

            // store input as the first char pressed
            string _input = Input.inputString.ToLower();
            char key = _input[0];

            // check if dictionary has they char
            if (keyValuePairs.ContainsKey(key))
            {
                // whack the hole associated with the key
                keyValuePairs[key].WhackHole();
            }
        }
    }
}
