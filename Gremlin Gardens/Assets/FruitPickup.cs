using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{
    public bool beingCarried = false;
    public Transform CarriedFruit;  //transform in front of player where fruit stays
    public Transform CarriedGremlin;
    public GameObject player;     //used to determine distance
    public GameObject PickupIndicator;  //button prompt to pick up 
    public GameObject DropIndicator;    //button prompt to drop down
    public GameObject Canvas;
    public float shrinkRate = 0.005f;

    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    private bool beingEaten = false; //true if a gremlin is eating the fruit
    private GameObject currentGremlin; //gremlin that is looked at


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Canvas = GameObject.Find("Canvas");
        DropIndicator = Canvas.transform.Find("Fruit Drop").gameObject;
        PickupIndicator = Canvas.transform.Find("Fruit Pickup").gameObject;
        CarriedGremlin = player.transform.Find("Carried Gremlin");
        CarriedFruit = player.transform.Find("Carried Fruit");

        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (beingCarried)
        {
            PickupIndicator.SetActive(false);
            DropIndicator.SetActive(true);

            //drop fruit
            if (Input.GetKeyDown("q"))
            {
                this.transform.parent = null;
                //drop object back down. look into teleporting onto ground
                GetComponent<Rigidbody>().useGravity = true;
                beingCarried = false;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Collider>().enabled = true;
                DropIndicator.SetActive(false);
            }
        }

        //replace behavior with animations later
        if (beingEaten)
        {
            //shrink food object     
            Vector3 scaleChange = new Vector3(shrinkRate, shrinkRate, shrinkRate);
            this.gameObject.transform.localScale -= scaleChange * Time.deltaTime;
        }
        //delete object after it shrinks
        if (this.gameObject.transform.localScale.y < 0.1f)
            Destroy(gameObject);

        //distance between particular fruit and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Gremlin")
        {
            Transform newParent = other.transform.GetChild(0).transform;
            this.transform.position = newParent.position;
            this.transform.parent = newParent;
            beingCarried = false;
            beingEaten = true;
        }
    }

    private void OnMouseOver()
    {
        if (distanceFromPlayer < 3 && !beingEaten)
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

    private void OnMouseExit()
    {
        onFruit = false;
        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
        GetComponent<Outline>().OutlineWidth = 0;
    }
}
