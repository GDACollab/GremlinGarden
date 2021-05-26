using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be attached to fruit
public class EatAmbient : MonoBehaviour
{
    public float shrinkRate = 0.01f;
    public float maxStatVal;

    private Gremlin gremlin;
    private FoodObject fruit;
    private bool beingEaten = false;

    // Start is called before the first frame update
    void Start()
    {
        fruit = this.GetComponent<FoodObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // add stat changes
        //shrink food object     
        if (beingEaten)
        {
            Vector3 scaleChange = new Vector3(shrinkRate, shrinkRate, shrinkRate);
            this.gameObject.transform.localScale -= scaleChange * Time.deltaTime;

            if (this.gameObject.transform.localScale.y < 0.00005f)
            {
                maxStatVal = gremlin.maxStatVal;
                string stat = determineStat(fruit.foodName);

                // don't go over max stat value
                float statChange = gremlin.getStat(stat) + fruit.food.getStatAlteration(stat);
                if (statChange > maxStatVal)
                {
                    statChange = maxStatVal;
                }
                // string, float
                gremlin.setStat(stat, statChange);
                gremlin.setStat("Happiness", 1 + gremlin.getStat("Happiness"));
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Gremlin")
        {
            beingEaten = true;
            gremlin = other.GetComponent<GremlinObject>().gremlin;
            AudioSource gremlinSound = other.gameObject.GetComponent<AudioSource>();
            gremlinSound.Play();
        }
    }

    private string determineStat(string food)
    {
        string stat = "";
        switch (food)
        {
            case "Apple":
                stat = "Stamina";
                break;
            case "Cheetah Fruit":
                stat = "Running";
                break;
            case "Monkey Fruit":
                stat = "Climbing";
                break;
            case "Dolphin Fruit":
                stat = "Swimming";
                break;
            case "Dragon Fruit":
                stat = "Flying";
                break;
            default:
                break;
        }
        return stat;
    }
}
