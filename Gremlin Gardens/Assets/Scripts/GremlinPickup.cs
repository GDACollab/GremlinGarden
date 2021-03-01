using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinPickup : MonoBehaviour
{
    public Transform CarriedGremlin;  //transform in front of player where Gremlin stays
    public GameObject player;     //used to determine distance
    public Transform CarriedFruit;
    public GameObject PickupIndicator;  //button prompt to pick up 
    public GameObject DropIndicator;    //button prompt to drop down
    public bool beingCarried = false;

    private bool onGremlin;
    private float distanceFromPlayer;
    private bool eClicked = false;
    private double eDownTime = 0;
    private bool canPickUp = false; //turns true when e has been held long enough over the gremlin
    public void Start()
    {
        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown("q") && beingCarried)
        {
            this.transform.parent = null;
            //drop object back down. look into teleporting onto ground
            GetComponent<Rigidbody>().useGravity = true;
            beingCarried = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            DropIndicator.SetActive(false);
            GetComponent<Collider>().enabled = true;
            eDownTime = 0;
            canPickUp = false;
        }
        //keep track of how long button has been pressed to use for picking up
        //first click
        if (Input.GetKeyDown("e") && onGremlin)
        {
            eDownTime = 0;
            eClicked = true;
        }
        //key down
        if (eClicked && Input.GetKey("e") && onGremlin)
        {
            eDownTime += Time.deltaTime;
            //update UI indicator here...


        }
        //if down for x seconds
        if (eDownTime >= .25)
        {
            canPickUp = true;
        }
        //distance between particular Gremlin and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
    }

    private void OnMouseOver()
    {
        //close enough to player but not too far away
        //idk how this will scale
        if (distanceFromPlayer < 3)
        {
            onGremlin = true;
            //indicate that gremlin can be picked up
            //highlighting gremlin maybe? ask design
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
            //PET
            if (!canPickUp && !beingCarried && Input.GetKeyUp("e"))
            {
                //actually pet


                //cancel holding
                eClicked = false;
                eDownTime = 0;

            }
            //PICKUP
            if (canPickUp && !beingCarried && CarriedGremlin.childCount == 0 && CarriedFruit.childCount == 0)
            {
                //remove gravity so object isnt spazzing out
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = CarriedGremlin.position;
                this.transform.parent = GameObject.Find("Carried Gremlin").transform;
                beingCarried = true;
                GetComponent<Collider>().enabled = false;
                PickupIndicator.SetActive(false);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
        //not close enough to pick up
        else
        {
            onGremlin = false;
            PickupIndicator.SetActive(false);
            GetComponent<Outline>().OutlineWidth = 0;
        }
    }
    private void OnMouseExit()
    {
        onGremlin = false;
        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
        GetComponent<Outline>().OutlineWidth = 0;
        eDownTime = 0;
        eClicked = false;
    }
}