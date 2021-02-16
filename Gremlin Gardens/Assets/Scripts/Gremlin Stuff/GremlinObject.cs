﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class attatched to each Gremlin gameObject
public class GremlinObject : MonoBehaviour
{
    // Variables for the in-game gremlins
    [HideInInspector]
    public Gremlin gremlin;
    Rigidbody body;
    public string gremlinName;
    
    //Use Awake instead of Start so that we get the stats ASAP. Useful for preventing bugs where the Gremlin is spawned in immediately but does not have stats yet.
    void Awake()
    {
        // Defines the Gremlin
        gremlin = new Gremlin(gremlinName);
        //Temporary values to work with the track system.
        gremlin.setStat("Running", 1);
        gremlin.setStat("Flying", 1);
        gremlin.setStat("Stamina", 1);
        gremlin.setStat("Climbing", 1);
        gremlin.setStat("Happiness", 1);
        gremlin.setStat("Swimming", 1);
        //Adds Necessary Collision Components
        gameObject.AddComponent<SphereCollider>();
        body = gameObject.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}