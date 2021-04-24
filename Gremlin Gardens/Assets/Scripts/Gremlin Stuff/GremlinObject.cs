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
    }

    /// <summary>
    /// Used for transferring gremlins across scenes.
    /// </summary>
    /// <param name="srcGremlin">The source gremlin from which to copy.</param>
    public void CopyGremlinData(GremlinObject srcGremlin) {
        gremlinName = srcGremlin.gremlinName;
        foreach (KeyValuePair<string, float> statistic in gremlin.getStats()) {
            gremlin.setStat(statistic.Key, srcGremlin.gremlin.getStat(statistic.Key));
        }
    }
}
