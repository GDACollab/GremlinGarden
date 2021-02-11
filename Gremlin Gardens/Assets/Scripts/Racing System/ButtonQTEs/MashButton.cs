using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MashButton : QTEScript
{
    /// <summary>
    /// Key to be mashing.
    /// </summary>
    [Tooltip("Key to be mashing.")]
    public string key = "";

    /// <summary>
    /// How much to increase the gremlin's relative speed by.
    /// </summary>
    [Tooltip("How much to increase the gremlin's base speed by.")]
    public float speedIncrease = 3f;

    /// <summary>
    /// How much to decrease the gremlin's speed over time by.
    /// </summary>
    [Tooltip("How much to decrease the gremlin's speed over time by.")]
    public float speedDecrease = 0.5f;

    /// <summary>
    /// The speed that we're going to add to activeModule.modifiedSpeed in ModifySpeed().
    /// </summary>
    float speedChange;

    /// <summary>
    /// First sprite in the animation
    /// </summary>
    public Sprite firstSprite;
    /// <summary>
    /// Second sprite in the animation.
    /// </summary>
    public Sprite secondSprite;

    /// <summary>
    /// The frame rate of the animation.
    /// </summary>
    [Tooltip("The Frame Rate of the animation. Not really a frame rate, it's based on Time.deltaTime, but... whatever.")]
    public float frameRate = 1;

    float frames;

    [HideInInspector]
    public bool isKeyDown;
    // Start is called before the first frame update
    void Start()
    {
        isKeyDown = false;
        speedChange = 0;
        frames = 0;
    }

    public override float ModifySpeed() //Just increase the speed by however much we've calculated it.
    {
        return speedChange;
    }

    // Update is called once per frame
    void Update()
    {
        if (speedChange > 0) {
            speedChange -= speedDecrease;
        }
        if (Input.GetKeyDown(key) && isKeyDown == false)
        {
            isKeyDown = true;
            speedChange += speedIncrease;
        }
        if (Input.GetKeyUp(key) && isKeyDown == true)
        {
            isKeyDown = false;
        }
        frames += Time.deltaTime;
        if (frames > frameRate)
        {
            frames = 0;
            if (GetComponent<Image>().sprite == firstSprite)
            {
                GetComponent<Image>().sprite = secondSprite;
            }
            else
            {
                GetComponent<Image>().sprite = firstSprite;
            }
        }
    }
}
