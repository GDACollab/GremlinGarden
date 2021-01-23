using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndPet : MonoBehaviour
{
    public Transform theCarried;  //transform in front of player where Gremlin stays
    public GameObject player;     //used to determine distance
    public GameObject TextIndicator;  //button prompt to pick up 
    public bool beingCarried = false;

    private float distanceFromPlayer;
    private bool eClicked = false;
    private double eDownTime = 0;
    private bool canPickUp = false; //turns true when e has been held long enough over the gremlin
    public void Start()
    {
        TextIndicator.SetActive(false);
    }

    public void Update(){
        if (Input.GetKeyDown("q") && beingCarried){
            this.transform.parent = null;
            //drop object back down. look into teleporting onto ground
            GetComponent<Rigidbody>().useGravity = true;
            beingCarried = false;
            TextIndicator.SetActive(true);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            eDownTime = 0;
            canPickUp = false;
        }
        //keep track of how long button has been pressed to use for picking up
        //first click
        if (Input.GetKeyDown("e")) {
            eDownTime = 0;
            eClicked = true;
        }
        //key down
        if (eClicked && Input.GetKey("e")) {
            eDownTime += Time.deltaTime;
            //update UI indicator here...


        }
        //if down for x seconds
        if (eDownTime >= .25) {
            canPickUp = true;
        }
        //distance between particular Gremlin and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
    }

    private void OnMouseOver(){
        //close enough to player but not too far away
        //idk how this will scale
        if (distanceFromPlayer < 3) {
            //indicate that gremlin can be picked up
            //highlighting gremlin maybe? ask design
            GetComponent<Outline>().OutlineWidth = 10;
            if (beingCarried) {
                GetComponent<Outline>().OutlineWidth = 0;
                TextIndicator.SetActive(false);
            }
            else {
                TextIndicator.SetActive(true);
            }
            //pet
            if(!canPickUp && !beingCarried){
                if (Input.GetKeyUp("e")){
                    //actually pet


                    //cancel holding
                    eClicked = false;
                    eDownTime = 0;
                }
            }
            //pickup
            if (canPickUp && !beingCarried) {
                //remove gravity so object isnt spazzing out
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = theCarried.position;
                this.transform.parent = GameObject.Find("Carried").transform;
                beingCarried = true;
                TextIndicator.SetActive(false);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        }
        //not close enough to pick up
        else {
            TextIndicator.SetActive(false);
            GetComponent<Outline>().OutlineWidth = 0;
        }
    }
    private void OnMouseExit(){
        TextIndicator.SetActive(false);
        GetComponent<Outline>().OutlineWidth = 0;
        eDownTime = 0;
        eClicked = false;
    }
}