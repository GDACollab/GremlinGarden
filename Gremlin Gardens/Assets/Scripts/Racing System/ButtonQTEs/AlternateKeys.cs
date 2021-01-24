using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// First key's image to change.
    /// </summary>
    [Tooltip("First key's image to change.")]
    public Image firstKeyImage;
    /// <summary>
    /// Second key's image to change.
    /// </summary>
    [Tooltip("Second key's image to change.")]
    public Image secondKeyImage;

    /// <summary>
    /// First frame to use.
    /// </summary>
    [Tooltip("First frame to use.")]
    public Sprite firstSprite;
    /// <summary>
    /// Second frame to use.
    /// </summary>
    [Tooltip("Second frame to use.")]
    public Sprite secondSprite;

    /// <summary>
    /// The frame rate of the animation.
    /// </summary>
    [Tooltip("The Frame Rate of the animation. Not really a frame rate, it's based on Time.deltaTime, but... whatever.")]
    public float frameRate = 1;

    float firstKeyFrames = 0.5f;
    float secondKeyFrames = 0;

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
        firstKeyFrames += Time.deltaTime;
        secondKeyFrames += Time.deltaTime;
        if (firstKeyFrames > frameRate)
        {
            firstKeyFrames = 0;
            if (firstKeyImage.sprite == firstSprite)
            {
                firstKeyImage.sprite = secondSprite;
            }
            else {
                firstKeyImage.sprite = firstSprite;
            }
        }
        if (secondKeyFrames > frameRate) {
            secondKeyFrames = 0;
            if (secondKeyImage.sprite == firstSprite)
            {
                secondKeyImage.sprite = secondSprite;
            }
            else {
                secondKeyImage.sprite = firstSprite;
            }
        }
    }
}
