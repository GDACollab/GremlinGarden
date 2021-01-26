using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Object
{
    // Database of all possible food
    // Indices [changed stat(s) & by how much, name, model] 
    public static readonly (Dictionary<string, float>, string, Object)[] allPossibleFood;

    // The stat(s) the food will alter and by how much
    private Dictionary<string, float> alteredStats;

    // The name of the food
    private string foodName;

    // The model used by the food in-game
    private Object model;

    public Food()
    {
        // The index of allPossibleFood from which the stats will be drawn from
        int foodIndex = Random.Range(0,allPossibleFood.Length);

        // Setting the values of the food's stats
        (alteredStats, foodName, model) = allPossibleFood[foodIndex];
    }
}
