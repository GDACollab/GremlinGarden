using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateKeys : QTEScript
{
    /// <summary>
    /// First key to press.
    /// </summary>
    [Tooltip("First key to press.")]
    public string firstKey = "";
    /// <summary>
    /// Second key to press.
    /// </summary>
    [Tooltip("Second key to press.")]
    public string secondKey = "";

    /// <summary>
    /// How much to increase the speed by.
    /// </summary>
    [Tooltip("How much to increase the speed by.")]
    public float speedIncrease = 3f;


    /// <summary>
    /// How much to decrease the speed by over time.
    /// </summary>
    [Tooltip("How much to decrease the speed by over time.")]
    public float speedDecrease = 0.5f;

    bool firstKeyDown = false;
    bool secondKeyDown = false;
    int keyToPress = 0;

    float speedChange = 0;

    public override float ModifySpeed()
    {
        return speedChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(firstKey) && firstKeyDown == false) {
            firstKeyDown = true;
            if (keyToPress == 0)
            {
                keyToPress = 1;
                speedChange += speedIncrease;
            }
        }
        if (Input.GetKeyUp(firstKey) && firstKeyDown == true) {
            firstKeyDown = false;
        }
        if (Input.GetKeyDown(secondKey) && secondKeyDown == false)
        {
            secondKeyDown = true;
            if (keyToPress == 1)
            {
                keyToPress = 0;
                speedChange += speedIncrease;
            }
        }
        if (Input.GetKeyUp(secondKey) && secondKeyDown == true) {
            secondKeyDown = false;
        }
    }
}
