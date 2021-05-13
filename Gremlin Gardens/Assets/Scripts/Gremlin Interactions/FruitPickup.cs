using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{

    public float shrinkRate = 0.01f;
    public float maxStatVal;
    public float pickupDistance = 6f;

    private Gremlin gremlin;
    private GameObject gremlinGameObj;
    private FoodObject fruit;
    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    private bool beingEaten = false; //true if a gremlin is eating the fruit
    private bool isEating = false; //true if the gremlin is currently eating, used for the animator
    private bool beingCarried = false;
    private Transform CarriedFruit;  //transform in front of player where fruit stays
    private Transform CarriedGremlin;
    private GameObject player;     //used to determine distance
    private GameObject PickupIndicator;  //button prompt to pick up 
    private GameObject DropIndicator;    //button prompt to drop down
    private GameObject Canvas;
    private AudioSource pickUpSound;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Canvas = GameObject.Find("Canvas (Hub UI)");
        GameObject interactions = Canvas.transform.GetChild(1).gameObject;
        DropIndicator = interactions.transform.Find("Fruit Drop").gameObject;
        PickupIndicator = interactions.transform.Find("Fruit Pickup").gameObject;
        CarriedGremlin = player.transform.Find("Carried Gremlin");
        CarriedFruit = player.transform.Find("Carried Fruit");
        fruit = this.GetComponent<FoodObject>();
        pickUpSound = this.GetComponent<AudioSource>();

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
            Debug.Log("shrinking fruit");
            Debug.Log(this.transform.localScale);

            //delete object after it shrinks
            if (this.transform.localScale.y < 0.0005f)
            {
                Debug.Log("made it here");
                //set stats
                maxStatVal = gremlin.maxStatVal;
                string stat = determineStat(fruit.foodName);
                float statChange = gremlin.getStat(stat) + (15 * (gremlin.getStat("Happiness") + 1)); //Old formula: + fruit.food.getStatAlteration(stat);
                if (statChange > maxStatVal)
                    statChange = maxStatVal;
                gremlin.setStat(stat, statChange);

                //done eating, destroy game object and re-enable ai
                Destroy(gameObject);
                Debug.Log("Fruit deleted");
                gremlinGameObj.GetComponent<GremlinAI>().enabled = true;
            }
        }

        //distance between particular fruit and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Gremlin")
        {
            //get reference to gremlin holding the food
            gremlinGameObj = other;
            //position the food
            Transform newParent = other.GetComponent<GremlinHand>().hand.transform;
            //Transform newParent = other.transform.GetChild(1).transform;

            this.transform.position = newParent.position;
            Debug.Log(this.transform.localScale);
            //this.transform.localScale = Vector3.Scale(this.transform.localScale, new Vector3(10, 10, 10)); //TODO: temp fix for blender ecport issue
            this.transform.parent = newParent;
            //get reference to Gremlin script
            gremlin = other.GetComponent<GremlinObject>().gremlin;
            beingCarried = false;
            beingEaten = true;
            //disable ai and collision box (collision was causing gremlins to slide around), play eating sound, and play eating animation
            GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<GremlinAI>().enabled = false;
            other.GetComponentInChildren<GremlinAudioController>().PlayEat();
            other.transform.Find("gremlinModel").GetComponent<Animator>().SetTrigger("isEating");
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
                pickUpSound.Play();
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
