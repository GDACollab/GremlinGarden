using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Food : Object
{
    // Database of all possible food
    // Indices [changed stat(s) & by how much, name, model] 
    public static  Dictionary<string, Food> allPossibleFood = new Dictionary<string, Food>() {
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

    //Contructions Non-Random Food Object
    public Food(GameObject model, string foodName, Dictionary<string, float> alteredStats)
    {
        this.model = model;
        this.foodName = foodName;
        this.alteredStats = alteredStats;
    }

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

    public GameObject getModel()
    {
        return model;
    }

    public string getName()
    {
        return name;
    }

    public Dictionary<string, float> getStats()
    {
        return alteredStats;
    }


}
