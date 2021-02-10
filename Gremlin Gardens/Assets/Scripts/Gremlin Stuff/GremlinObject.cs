using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class attatched to each Gremlin gameObject
public class GremlinObject : MonoBehaviour
{
    // Variables for the in-game gremlins
    public Gremlin gremlin;
    Rigidbody body;
    string gremlinName;
    
    // Start is called before the first frame update
    void Start()
    {
        // Defines the Gremlin
        gremlin = new Gremlin(gremlinName);

        //Adds Necessary Collision Components
        gameObject.AddComponent<SphereCollider>();
        body = gameObject.AddComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
