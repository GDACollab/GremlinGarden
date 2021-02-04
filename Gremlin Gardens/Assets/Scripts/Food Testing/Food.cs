using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Object
{
    // Database of all possible food
    // Indices [changed stat(s) & by how much, name] 
    public static readonly (Dictionary<string, double>, string)[] allPossibleFood = new(Dictionary<string, double>, string)[]
    {
        (new Dictionary<string, double>(){{"Happiness", 1.5}}, "Apple"),
        (new Dictionary<string, double>(){{"Swimming", 4.0}}, "Guava")
    };

    // The stat(s) the food will alter and by how much
    private Dictionary<string, double> alteredStats;

    // The name of the food
    private string foodName;

    public Food()
    {
        // The index of allPossibleFood from which the stats will be drawn from
        int foodIndex = Random.Range(0, allPossibleFood.Length);

        // Setting the values of the food's stats
        (alteredStats, foodName) = allPossibleFood[foodIndex];
    }

    /**
     * Returns how much a given stat will change when a gremlin eats the food
     * 
     * @param stat: the stat in question
     * @return: how much the given stat will change
     */
    public double getStatAlteration(string stat)
    {
        return alteredStats[stat];
    }

    /**
     * Returs a dictionary of all the stats that will be altered and how much they will be altered by
     * 
     * @return: a copy of alteredStats
     */
    public Dictionary<string, double> getAlteredStats()
    {
        return new Dictionary<string, double>(alteredStats);
    }

    // Returns the name of the food
    public string getName()
    {
        return foodName;
    }
}
