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
    // Dictionary of all gremlin stats and their values
    private Dictionary<string, float> gremlinStats = new Dictionary<string, float>()
    {
        {"Stamina", 0},
        {"Happiness", 0},
        {"Running", 0},
        {"Climbing", 0},
        {"Swimming", 0},
        {"Flying", 0}
    };

    public Gremlin(){}

    /*
     * Modifies the value of a specified stat to a given value
     * 
     * @param stat - The name of the stat 
     * @param value - The value that stat will be set to
     */
    public void setStat(string stat, float value)
    {
        gremlinStats[stat] = value;
    }

    /*
     * Gives the value of a specified stat
     * 
     * @param stat - The name of the stat 
     * @return - The value of the specified stat
     */
    public float getStat(string stat)
    {
        return gremlinStats[stat];
    }
}
