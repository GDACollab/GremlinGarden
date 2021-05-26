using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{

    public float shrinkRate = 0.01f;
    public float maxStatVal;
    public float pickupDistance = 6f;

    private Gremlin gremlin;
    private FoodObject fruit;
    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    private bool beingEaten = false; //true if a gremlin is eating the fruit
    private bool beingCarried = false;
    private Transform CarriedFruit;  //transform in front of player where fruit stays
    private Transform CarriedGremlin;
    private GameObject player;     //used to determine distance
    private GameObject PickupIndicator;  //button prompt to pick up 
    private GameObject DropIndicator;    //button prompt to drop down
    private GameObject Canvas;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Canvas = GameObject.Find("Canvas (Hub UI)");
        DropIndicator = Canvas.transform.Find("Fruit Drop").gameObject;
        PickupIndicator = Canvas.transform.Find("Fruit Pickup").gameObject;
        CarriedGremlin = player.transform.Find("Carried Gremlin");
        CarriedFruit = player.transform.Find("Carried Fruit");
        fruit = this.GetComponent<FoodObject>();

        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var playerMove = player.GetComponent<PlayerMovement>();
        if (playerMove.centeredObject == this.gameObject)
        {
            IsCentered();
        }
        else if (playerMove.centeredObject != this.gameObject && playerMove.hitObjectIsNew)
        {
            IsExited();
        }
        if (beingCarried)
        {
            PickupIndicator.SetActive(false);
            DropIndicator.SetActive(true);

            //drop fruit
            if (Input.GetKeyDown("q"))
            {
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                beingCarried = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Collider>().enabled = true;
            }
        }
        else
            DropIndicator.SetActive(false);

        //replace behavior with animations later
        if (beingEaten)
        {
            //shrink food object     
            Vector3 scaleChange = new Vector3(shrinkRate, shrinkRate, shrinkRate);
            this.gameObject.transform.localScale -= scaleChange * Time.deltaTime;
            //Debug.Log(this.gameObject.transform.localScale);

            //delete object after it shrinks
            if (this.gameObject.transform.localScale.y < 0.0005f)
            {
                maxStatVal = gremlin.maxStatVal;
                string stat = determineStat(fruit.foodName);
                float statChange = gremlin.getStat(stat) + fruit.food.getStatAlteration(stat);
                if (statChange > maxStatVal)
                    statChange = maxStatVal;
                gremlin.setStat(stat, statChange);
                gremlin.setStat("Happiness", 1 + gremlin.getStat("Happiness"));
                Destroy(gameObject);
            }
        }

        //distance between particular fruit and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Gremlin" && beingCarried)
        {
            Transform newParent = other.transform.GetChild(1).transform;
            this.transform.position = newParent.position;
            this.transform.parent = newParent;
            gremlin = other.GetComponent<GremlinObject>().gremlin;
            beingCarried = false;
            beingEaten = true;
            AudioSource gremlinSound = other.gameObject.GetComponent<AudioSource>();
            gremlinSound.Play();
        }
    }

    private void IsCentered()
    {
        if (distanceFromPlayer < pickupDistance && !beingEaten)
        {
            onFruit = true;
            GetComponent<Outline>().OutlineWidth = 10;
            if (CarriedGremlin.childCount != 0 || CarriedFruit.childCount != 0)
                GetComponent<Outline>().OutlineWidth = 0;
            else
                PickupIndicator.SetActive(true);

            //PICKUP FRUIT
            if (!beingCarried && Input.GetKeyDown("e") && CarriedFruit.childCount == 0 && CarriedGremlin.childCount == 0)
            {
                //remove gravity so object isnt spazzing out
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = CarriedFruit.position;
                this.transform.parent = GameObject.Find("Carried Fruit").transform;
                beingCarried = true;
                PickupIndicator.SetActive(false);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                //GetComponent<Collider>().enabled = false;
            }
        }
    }

    private void IsExited()
    {
        onFruit = false;
        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
        GetComponent<Outline>().OutlineWidth = 0;
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
