using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class attatched to each Gremlin gameObject
public class GremlinObject : MonoBehaviour
{
    // Variables for the in-game gremlins
    [HideInInspector]
    public Gremlin gremlin;
    private Rigidbody body;
    public string gremlinName;
    private float speedMultiplier = 6f;

    // Start is called before the first frame update
    void Start()
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
        //Adds Necessary Collision Components
        gameObject.AddComponent<BoxCollider>();
        body = gameObject.AddComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        FoodObject[] allFoods = FindObjectsOfType<FoodObject>();
        GameObject nearestFood = null;
        Vector3 position = transform.position;
        foreach (FoodObject foodObject in allFoods)
        {
            if (nearestFood == null || Vector3.Distance(position, nearestFood.transform.position) >
                Vector3.Distance(position, foodObject.transform.position))
            {
                nearestFood = foodObject.gameObject;
            }
        }

        if (nearestFood != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(nearestFood.transform.position - this.gameObject.transform.position);
            Quaternion turnTowards = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            turnTowards = Quaternion.Euler(0, turnTowards.eulerAngles.y, 0);
            body.MoveRotation(turnTowards);

            body.velocity = transform.forward * (1f + (gremlin.getStat("Running")- 1f) / 2f) * speedMultiplier;
        } else
        {
            body.velocity = Vector3.zero;
        }
    }

    public void EatFood(Food food)
    {
        Dictionary<string, float> stats = food.getStats();
        foreach (KeyValuePair<string, float> kvp in stats)
        {
            gremlin.incrementStat(kvp.Key, kvp.Value);
        }
        Debug.Log($"These are {gremlinName}'s stats:");
        foreach (KeyValuePair<string, float> kvp in gremlin.getStats())
        {
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        }

        body.velocity = Vector3.zero;
    }
}
