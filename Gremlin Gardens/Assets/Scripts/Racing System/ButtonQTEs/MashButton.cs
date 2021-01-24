using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashButton : QTEScript
{
    /// <summary>
    /// Key to be mashing.
    /// </summary>
    [Tooltip("Key to be mashing.")]
    public string key = "";

    /// <summary>
    /// How much to increase the gremlin's distance by.
    /// </summary>
    [Tooltip("How much to increase the gremlin's relative distance by.")]
    public float distanceIncrease = 0.01f;

    [HideInInspector]
    public bool isKeyDown;
    // Start is called before the first frame update
    void Start()
    {
        isKeyDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && isKeyDown == false)
        {
            isKeyDown = true;
            Debug.Log(activeModule.totalDistance);
            activeModule.totalDistance += distanceIncrease;
        }
        if (Input.GetKeyUp(key) && isKeyDown == true)
        {
            isKeyDown = false;
        }
    }
}
