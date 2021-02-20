﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinPickup : MonoBehaviour
{
    public Transform CarriedGremlin;  //transform in front of player where Gremlin stays
    public GameObject player;     //used to determine distance
    public Transform CarriedFruit;
    public GameObject PickupIndicator;  //button prompts when gremlin is on the ground
    public GameObject DropIndicator;    //button prompts when carrying gremlin
    public GameObject StatMenu;
    public GameObject Canvas;
    public bool beingCarried = false;
    public float jumpHeight = 1.0f; //for temporary pet behaviour
    public float petCooldown = 1.0f; //time for petting action to reset
    public float cuddleCooldown = 2.0f; //time for petting action to reset
    public int petIncrease = 1; //how much to increase happiness stat from petting
    public int cuddleIncrease = 2; //how much to increase happiness stat from cuddling
    public bool enableStatMenu = false;

    private bool onGremlin;
    private float distanceFromPlayer;
    private bool eClicked = false;
    private double eDownTime = 0;
    private bool canPickUp = false; //turns true when e has been held long enough over the gremlin
    private bool beingPet = false;
    private bool beingCuddled = false;
    private float petCooldownTimer = 0.0f; //timer for pet cooldown
    private float cuddleCooldownTimer = 0.0f; //timer for pet cooldown

    private Rigidbody rb;
    //private Gremlin gremlin;

    public void Start()
    {

        player = GameObject.Find("Player");
        Canvas = GameObject.Find("Canvas");
        StatMenu = Canvas.transform.Find("Stat Menu").gameObject;
        DropIndicator = Canvas.transform.Find("Gremlin Carry").gameObject;
        PickupIndicator = Canvas.transform.Find("Gremlin Pickup").gameObject;
        CarriedGremlin = player.transform.Find("Carried Gremlin");
        CarriedFruit = player.transform.Find("Carried Fruit");

        PickupIndicator.SetActive(false);
        DropIndicator.SetActive(false);
        StatMenu.SetActive(false);
        rb = GetComponent<Rigidbody>();
        GetComponent<Outline>().OutlineWidth = 0;
        //gremlin = GetComponent<Gremlin>();   
    }

    public void Update()
    {
        //distance between particular Gremlin and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        //interactions when carrying gremlin
        if (beingCarried && !beingCuddled)
        {
            //PUT DOWN
            if (Input.GetKeyDown("q"))
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

            //CUDDLE
            if (Input.GetKeyDown("e"))
            {
                beingCuddled = true;
                cuddleCooldownTimer = 0.0f;

                //temp cuddle behavior
                var temp = this.GetComponent<Renderer>();
                temp.material.SetColor("_Color", Color.cyan);

                //gremlin.setStat("Happiness", cuddleIncrease + getStat("Happiness"));
            }

            //YEET THAT BITCH
            /*if(Input.GetMouseButtonDown(0))
            {

            }*/
        }

        //keep track of how long button has been pressed to use for picking up
        //first click
        if (Input.GetKeyDown("e") && onGremlin)
        {
            eDownTime = 0;
            eClicked = true;
        }
        //key down
        if (eClicked && Input.GetKey("e") && onGremlin && !beingPet)
        {
            eDownTime += Time.deltaTime;
            //update UI indicator here...

        }
        //if down for x seconds
        if (eDownTime >= .25)
            canPickUp = true;

        //pet cooldown
        if (beingPet)
            petCooldownTimer += Time.deltaTime;
        if (petCooldownTimer >= petCooldown)
        {
            beingPet = false;
            petCooldownTimer = 0.0f;
        }
        //cuddle cooldown
        if (beingCuddled)
            cuddleCooldownTimer += Time.deltaTime;
        if (cuddleCooldownTimer >= cuddleCooldown)
        {
            beingCuddled = false;
            cuddleCooldownTimer = 0.0f;
            //temp cuddle behavior
            var temp = this.GetComponent<Renderer>();
            temp.material.SetColor("_Color", Color.green);
        }

        //set correct text prompts
        if (beingCarried && !beingCuddled)
        {
            PickupIndicator.SetActive(false);
            DropIndicator.SetActive(true);
        }
        if (beingCuddled)
            DropIndicator.SetActive(false);
        if (beingPet)
            PickupIndicator.SetActive(false);

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
            if (CarriedGremlin.childCount != 0 || beingPet || CarriedFruit.childCount != 0)
                GetComponent<Outline>().OutlineWidth = 0;
            else
                PickupIndicator.SetActive(true);

            //PET
            if (!canPickUp && !beingCarried && Input.GetKeyUp("e") && !beingPet)
            {
                beingPet = true;
                petCooldownTimer = 0.0f;

                //actually pet
                //gremlin.setStat("Happiness", petIncrease + getStat("Happiness"));

                //jumps happily as temp behaviour until we get animations
                rb.AddForce(Vector3.up * jumpHeight * 100);

                //lock player in place?

                //cancel holding
                eClicked = false;
                eDownTime = 0;

            }
            //PICKUP
            if (canPickUp && !beingCarried && !beingPet && CarriedGremlin.childCount == 0 && CarriedFruit.childCount == 0)
            {
                //remove gravity so object isnt spazzing out
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = CarriedGremlin.position;
                this.transform.parent = CarriedGremlin;
                beingCarried = true;
                GetComponent<Collider>().enabled = false;
                PickupIndicator.SetActive(false);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

            //STAT MENU
            if (!canPickUp && !beingCarried && Input.GetKeyDown("q") && !beingPet)
            {
                enableStatMenu = !enableStatMenu;
                StatMenu.SetActive(enableStatMenu);
            }


        }
        //not close enough to pick up
        else
        {
            onGremlin = false;
            PickupIndicator.SetActive(false);
            GetComponent<Outline>().OutlineWidth = 0;
            enableStatMenu = false;
            StatMenu.SetActive(enableStatMenu);
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
        enableStatMenu = false;
        StatMenu.SetActive(enableStatMenu);
    }


}