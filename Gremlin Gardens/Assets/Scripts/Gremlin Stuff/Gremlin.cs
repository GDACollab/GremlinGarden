using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that defines and alters each Gremlin's stats
public class Gremlin : Object
{
    public float maxStatVal = 1000.0f;

    //Name of the gremlin
    private string gremlinName;

    /// <summary>
    /// We store this so the GremlinSpawner can place the gremlin back where it was. Used by RaceSelection before loading a new scene (see MouseDown in RaceSelection.cs)
    /// </summary>
    public Vector3 currentPosition;

    public Quaternion currentRotation;

    public Color gremColor;

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
        Debug.Log(gremlinName);
        return gremlinName;
    }

    /*
    * Sets gremlinName to a new value
    */
    public void setName(string newName)
    {
        gremlinName = newName;
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

    /*
     * Increments a given stat by a certain amount
     * 
     * @param stat - The name of the stat 
     * @param amount - The amount to increase the stat by
     */
    public void incrementStat(string stat, float amount)
    {
        gremlinStats[stat] = gremlinStats[stat] + amount;
    }

    public Dictionary<string, float> getStats()
    {
        return new Dictionary<string, float>(gremlinStats);
    }


    /// <summary>
    /// Initialize the gremlin's relevant stats. Should be called by GremlinSpawner.
    /// </summary>
    public void InitializeGremlin()
    {
        setStat("Running", 0);
        setStat("Flying", 0);
        setStat("Stamina", 0);
        setStat("Climbing", 0);
        setStat("Happiness", 0);
        setStat("Swimming", 0);
    }

    /// <summary>
    /// Used for transferring gremlins across scenes.
    /// </summary>
    /// <param name="srcGremlin">The source gremlin from which to copy.</param>
    public void CopyGremlinData(Gremlin srcGremlin)
    {
        gremlinName = srcGremlin.gremlinName;
        currentPosition = srcGremlin.currentPosition;
        currentRotation = srcGremlin.currentRotation;
        gremColor = srcGremlin.gremColor;
        foreach (KeyValuePair<string, float> statistic in getStats())
        {
            setStat(statistic.Key, srcGremlin.getStat(statistic.Key));
        }
    }
}
