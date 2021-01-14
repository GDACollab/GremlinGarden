using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class attatched to each Gremlin gameObject
public class GremlinClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// Class that defines and alters each Gremlin's stats
public class Gremlin : Object
{
    // Array of all stats associated with each gremlin
    private float[] gremlinStats;

    // Dictionary of all indices in gremlinStats with a key of what stat that index represents
    public static readonly Dictionary<string, int> gremlinStatIndices = new Dictionary<string, int>()
    {
        {"Stamina", 0},
        {"Happiness", 1},
        {"Running", 2},
        {"Climbing", 3},
        {"Swimming", 4},
        {"Flying", 5}
    };

    public Gremlin()
    {
        gremlinStats = new float[gremlinStatIndices.Count];
    }

    /*
     * Modifies the value of a specified stat to a given value
     * 
     * @param stat - The name of the stat 
     * @param value - The value that stat will be set to
     */
    public void setStat(string stat, float value)
    {
        gremlinStats[gremlinStatIndices[stat]] = value;
    }

    /*
     * Gives the value of a specified stat
     * 
     * @param stat - The name of the stat 
     * @return - The value of the specified stat
     */
    public float getStat(string stat)
    {
        return gremlinStats[gremlinStatIndices[stat]];
    }
}
