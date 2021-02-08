using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Food : Object
{
    // Database of all possible food
    // Indices [changed stat(s) & by how much, name, model] 
    public static readonly Dictionary<string, Food> allPossibleFood = new Dictionary<string, Food>() {
        {"Apple", new Food(Resources.Load("Apple") as GameObject, "Apple", new Dictionary<string, float>() {
            {"strength", 0.25f }, {"speed", 0f}
        })},
        {"Green Apple", new Food(Resources.Load("Green Apple") as GameObject, "Green Apple", new Dictionary<string, float>() {
            {"strength", 0f }, {"speed", 1000f}
        })}
    };

    // The stat(s) the food will alter and by how much
    private Dictionary<string, float> alteredStats;

    // The name of the food
    private string foodName;

    // The model used by the food in-game
    private GameObject model;

    // Constructor for a Non-Random Food object
    public Food(GameObject model, string foodName, Dictionary<string, float> alteredStats)
    {
        this.model = model;
        this.foodName = foodName;
        this.alteredStats = alteredStats;
    }

    // Constructor for a Random Food object
    public Food()
    {
        // The index of allPossibleFood from which the stats will be drawn from
        int foodIndex = Random.Range(0,allPossibleFood.Count);

        // Setting the values of the food's stats
        Food reference = allPossibleFood[allPossibleFood.Keys.ElementAt(foodIndex)];
        this.model = reference.model;
        this.foodName = reference.foodName;
        this.alteredStats = reference.alteredStats;
    }

    // Returns the model associated with the food
    public GameObject getModel()
    {
        return model;
    }

    // Returns the name of the food
    public string getName()
    {
        return foodName;
    }

    /**
     * Returs a dictionary of all the stats that will be altered and how much they will be altered by
     * 
     * @return: a copy of alteredStats
     */
    public Dictionary<string, float> getStats()
    {
        return new Dictionary<string, float>(alteredStats);
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
}
