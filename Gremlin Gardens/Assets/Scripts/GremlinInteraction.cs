﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GremlinInteraction : MonoBehaviour
{
    private Image ChargeFill;
    private bool beingCarried = false;
    public float jumpHeight = 1.0f; //for temporary pet behaviour
    public float petCooldown = 1.0f; //time for petting action to reset
    public float cuddleCooldown = 2.0f; //time for petting action to reset
    public float petIncrease = 1.0f; //how much to increase happiness stat from petting
    public float cuddleIncrease = 2.0f; //how much to increase happiness stat from cuddling
    public float yeetIncrease = 3.0f;
    public int tossForce = 300;

    public float speed = 2.0f; //Speed of charge bar fill
    public float waitTime = 4.0f; //Total Time to fill charge bar
    private float maxStatVal;

    private Transform CarriedGremlin;  //transform in front of player where Gremlin stays
    private GameObject player;     //used to determine distance
    private Transform CarriedFruit;
    private GameObject PickupIndicator;  //button prompts when gremlin is on the ground
    private GameObject DropIndicator;    //button prompts when carrying gremlin
    private GameObject PetIndicator;  //button prompts when gremlin is on the ground
    private GameObject CuddleIndicator;    //button prompts when carrying gremlin
    private GameObject TossIndicator;  //button prompts when gremlin is on the ground
    private GameObject StatIndicator;    //button prompts when carrying gremlin
    private GameObject StatMenu;
    private GameObject Canvas;
    private GameObject ChargeBar;
    private bool onGremlin;
    private bool attemptYeet = false;
    private float distanceFromPlayer;
    private bool eClicked = false;
    private double eDownTime = 0;
    private bool canPickUp = false; //turns true when e has been held long enough over the gremlin
    private bool beingPet = false;
    private bool beingCuddled = false;
    private float petCooldownTimer = 0.0f; //timer for pet cooldown
    private float cuddleCooldownTimer = 0.0f; //timer for pet cooldown
    private bool enableStatMenu = false;
    private float statChange = 0.0f;


    private Rigidbody rb;
    public Gremlin gremlin;

    [Header("Aesthetic Stuff")]
    /// <summary>
    /// The text the gremlin is displaying.
    /// </summary>
    [Tooltip("The text object for the gremlin to display.")]
    public TMP_Text nameText;

    public void Start()
    {

        player = GameObject.Find("Player");
        Canvas = GameObject.Find("Canvas (Hub UI)");
        StatMenu = Canvas.transform.GetChild(0).gameObject;
        DropIndicator = Canvas.transform.GetChild(1).gameObject;
        PickupIndicator = Canvas.transform.GetChild(2).gameObject;
        PetIndicator = Canvas.transform.GetChild(5).gameObject;
        StatIndicator = Canvas.transform.GetChild(8).gameObject;
        CuddleIndicator = Canvas.transform.GetChild(6).gameObject;
        TossIndicator = Canvas.transform.GetChild(7).gameObject;
        ChargeBar = Canvas.transform.GetChild(9).gameObject;
        ChargeFill = ChargeBar.transform.GetChild(1).GetComponent<Image>();
        CarriedGremlin = player.transform.GetChild(1);
        CarriedFruit = player.transform.GetChild(2);

        ChargeBar.SetActive(false);
        PickupIndicator.SetActive(false);
        PetIndicator.SetActive(false);
        StatIndicator.SetActive(false);
        DropIndicator.SetActive(false);
        TossIndicator.SetActive(false);
        CuddleIndicator.SetActive(false);
        StatMenu.SetActive(false);
        rb = GetComponent<Rigidbody>();
        GetComponent<Outline>().OutlineWidth = 0;

        gremlin = this.GetComponent<GremlinObject>().gremlin;
        maxStatVal = gremlin.maxStatVal;
    }

    public void Update()
    {
        // Set the name text to face the player.
        nameText.transform.rotation = Quaternion.LookRotation(this.transform.position - player.transform.position, player.transform.up);

        //distance between particular Gremlin and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        var playerMove = player.GetComponent<PlayerMovement>();
        if (playerMove.centeredObject == this.gameObject)
        {
            IsCentered();
        }
        else if (playerMove.centeredObject != this.gameObject && playerMove.hitObjectIsNew)
        {
            IsExited();
        }

        //interactions when carrying gremlin
        if (beingCarried && !beingCuddled)
        {
            //PUT DOWN
            if (Input.GetKeyDown("q") && !attemptYeet)
                DropGremlin();
                

            //CUDDLE
            if (Input.GetKeyDown("e") && !attemptYeet)
            {
                beingCuddled = true;
                cuddleCooldownTimer = 0.0f;
                CuddleIndicator.SetActive(false);
                statChange = cuddleIncrease + gremlin.getStat("Happiness");
                if (statChange > maxStatVal)
                    statChange = maxStatVal;
                gremlin.setStat("Happiness", statChange);
                UpdateStats();
            }

            //TOSS
            if (Input.GetMouseButton(0))
            {
                TossIndicator.SetActive(false);
                DropIndicator.SetActive(false);
                CuddleIndicator.SetActive(false);
                ChargeBar.SetActive(true);
                attemptYeet = true;
                StartCoroutine("StartFill");
            }
            if (attemptYeet && Input.GetMouseButtonUp(0))
            {
                TossIndicator.SetActive(true);
                DropIndicator.SetActive(true);
                CuddleIndicator.SetActive(true);

                if (ChargeFill.fillAmount > 0.75)
                {
                    CarriedGremlin.GetComponent<Collider>().enabled = false;
                    rb.constraints = RigidbodyConstraints.None;
                    rb.useGravity = true;
                    rb.AddForce(Vector3.up * tossForce);
                    GetComponent<Collider>().enabled = true;
                    StartCoroutine("enableCarryCollider");
                    ChargeBar.SetActive(false);


                    statChange = yeetIncrease + gremlin.getStat("Happiness");
                    if (statChange > maxStatVal)
                        statChange = maxStatVal;
                    gremlin.setStat("Happiness", statChange);
                    UpdateStats();
                }
                else
                {
                    attemptYeet = false;
                    DropGremlin();
                    ChargeBar.SetActive(false);

                    //can stats become negative???
                    statChange = gremlin.getStat("Happiness") - yeetIncrease;
                    if (statChange < 0)
                        statChange = 0;
                    gremlin.setStat("Happiness", statChange);
                    UpdateStats();
                }
            }
            if (!attemptYeet)
            {
                StopAllCoroutines();
                ChargeFill.fillAmount = 0.0f;
            }
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
            PetIndicator.SetActive(true);
            petCooldownTimer = 0.0f;
        }
        //cuddle cooldown
        if (beingCuddled)
            cuddleCooldownTimer += Time.deltaTime;
        if (cuddleCooldownTimer >= cuddleCooldown)
        {
            beingCuddled = false;
            cuddleCooldownTimer = 0.0f;
            CuddleIndicator.SetActive(true);
        }

        

    }

    private void IsCentered()
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
            {
                PickupIndicator.SetActive(true);
                PetIndicator.SetActive(true);
                StatIndicator.SetActive(true);
            }


            //PET
            if (!canPickUp && !beingCarried && Input.GetKeyUp("e") && !beingPet)
            {
                beingPet = true;
                petCooldownTimer = 0.0f;
                PetIndicator.SetActive(false);
                //actually pet
                statChange = petIncrease + gremlin.getStat("Happiness");
                if (statChange > maxStatVal)
                    statChange = maxStatVal;
                gremlin.setStat("Happiness", statChange);
                UpdateStats();
                //lock player in place?

                //cancel holding
                eClicked = false;
                eDownTime = 0;

            }

            //PICKUP
            if (canPickUp && !beingCarried && !beingPet && CarriedGremlin.childCount == 0 && CarriedFruit.childCount == 0)
            {
                //remove gravity so object isnt spazzing out
                rb.useGravity = false;
                this.transform.position = CarriedGremlin.position;
                this.transform.parent = CarriedGremlin;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                beingCarried = true;
                GetComponent<Collider>().enabled = false;
                PickupIndicator.SetActive(false);
                PetIndicator.SetActive(false);
                StatIndicator.SetActive(false);
                DropIndicator.SetActive(true);
                TossIndicator.SetActive(true);
                CuddleIndicator.SetActive(true);
            }

            //STAT MENU
            if (!canPickUp && !beingCarried && Input.GetKeyDown("q") && !beingPet)
            {
                enableStatMenu = !enableStatMenu;
                StatMenu.SetActive(enableStatMenu);
                UpdateStats();
            }


        }
        //not close enough to pick up
        else
        {
            onGremlin = false;
            PickupIndicator.SetActive(false);
            PetIndicator.SetActive(false);
            StatIndicator.SetActive(false);
            GetComponent<Outline>().OutlineWidth = 0;
            enableStatMenu = false;
            StatMenu.SetActive(enableStatMenu);
        }
    }
    private void IsExited()
    {
        onGremlin = false;
        PickupIndicator.SetActive(false);
        PetIndicator.SetActive(false);
        StatIndicator.SetActive(false);

        GetComponent<Outline>().OutlineWidth = 0;
        eDownTime = 0;
        eClicked = false;
        enableStatMenu = false;
        StatMenu.SetActive(enableStatMenu);
    }

    private void UpdateStats()
    {
        StatMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gremlin.getName();
        StatMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Stamina: " + gremlin.getStat("Stamina");
        StatMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Happiness: " + gremlin.getStat("Happiness");
        StatMenu.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Running: " + gremlin.getStat("Running");
        StatMenu.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Climbing: " + gremlin.getStat("Climbing");
        StatMenu.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Swimming: " + gremlin.getStat("Swimming");
        StatMenu.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "Flying: " + gremlin.getStat("Flying");
    }

    private void DropGremlin()
    {
        this.transform.parent = null;
        rb.useGravity = true;
        beingCarried = false;
        rb.constraints = RigidbodyConstraints.None;
        DropIndicator.SetActive(false);
        TossIndicator.SetActive(false);
        CuddleIndicator.SetActive(false);
        GetComponent<Collider>().enabled = true;
        eDownTime = 0;
        canPickUp = false;
    }

    IEnumerator StartFill()
    {
        while (true)
        {
            yield return ChangeFill(0.0f, 1.0f, waitTime);
            yield return ChangeFill(1.0f, 0.0f, waitTime);
        }
    }

    IEnumerator ChangeFill(float start, float end, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            ChargeFill.fillAmount = Mathf.Lerp(start, end, i);
            yield return null;
        }
    }

    IEnumerator enableCarryCollider()
    {
        yield return new WaitForSeconds(0.5f);
        CarriedGremlin.GetComponent<Collider>().enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Carried Gremlin" && attemptYeet)
        {
            attemptYeet = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            this.transform.position = CarriedGremlin.position;
            GetComponent<Collider>().enabled = false;
            TossIndicator.SetActive(true);
        }
    }

}