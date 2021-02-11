using System.Collections;
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
    
    // Start is called before the first frame update
    void Start()
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
