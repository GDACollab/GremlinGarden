using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickup : MonoBehaviour
{
    public Transform CarriedFruit;  //transform in front of player where fruit stays
    public bool beingCarried = false;
    public Transform CarriedGremlin;
    public GameObject player;     //used to determine distance
    public GameObject PickupIndicator;  //button prompt to pick up 
    public GameObject DropIndicator;    //button prompt to drop down


    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    private GameObject currentGremlin; //gremlin that is looked at


    // Start is called before the first frame update
    void Start()
    {
        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (beingCarried)
        {
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
            //distance between particular fruit and the player
            distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        }
    }
    private void OnMouseOver()
    {
        if (distanceFromPlayer < 3)
        {
            onFruit = true;
            GetComponent<Outline>().OutlineWidth = 10;
            if (beingCarried)
            {
                GetComponent<Outline>().OutlineWidth = 0;
                PickupIndicator.SetActive(false);
                DropIndicator.SetActive(true);

            }
            else
            {
                PickupIndicator.SetActive(true);
            }
            //PICKUP FRUIT
            if (!beingCarried && Input.GetKeyDown("e") && CarriedFruit.childCount == 0 && CarriedGremlin.childCount == 0)
            {
                //remove gravity so object isnt spazzing out
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = CarriedFruit.position;
                this.transform.parent = GameObject.Find("CarriedFruit").transform;
                beingCarried = true;
                PickupIndicator.SetActive(false);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Collider>().enabled = false;
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
