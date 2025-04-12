using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggWobble : MonoBehaviour
{
    public float amplitude = 0.1f;   // How far the egg moves
    public float frequency = 1f;    // How fast the egg moves

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        // Add slight randomness to the amplitude and frequency for visual variation
        amplitude += Random.Range(-.3f, .3f);
        frequency += Random.Range(-.3f, .3f);
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal and vertical wave movements
        float offsetX = Mathf.Cos(Time.time * frequency) * (amplitude / 2f); // Horizontal wobble
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;       // Vertical wobble

        // Apply the movement to the egg's position
        transform.position = initialPosition + new Vector3(offsetX, offsetY, 0f);

    }
}
