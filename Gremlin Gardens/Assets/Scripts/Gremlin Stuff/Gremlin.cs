using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that defines and alters each Gremlin's stats
public class Gremlin : Object
{
    //Name of the gremlin
    private string gremlinName;

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

    /*
     * Constructor
     * Sets the name of the gremlin
     * 
     * @param name = the name of the gremlin
     */
    public Gremlin(string name)
    {
        gremlinName = name;
    }

    /*
     * Gives the name of the gremlin
     *
     * @return - The name of the gremlin
     */
    public string getName()
    {
        return gremlinName;
    }

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
