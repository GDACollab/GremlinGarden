using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GremlinInteraction : MonoBehaviour
{

    public float happinessDecayTime = 45.0f;
    public float happinessDecayAmount = 0.5f;
    public float petCooldown = 1.0f; //time for petting action to reset
    public float cuddleCooldown = 2.0f; //time for petting action to reset
    public float petIncrease = 0.05f; //how much to increase happiness stat from petting
    public float cuddleIncrease = 0.05f; //how much to increase happiness stat from cuddling
    public float yeetIncrease = 0.1f;
    public int tossForce = 300;
    public float maxHappinessVal = 1.0f;

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
    private GremlinAudioController audioController;
    private bool onGremlin;
    public bool attemptYeet = false;
    private float distanceFromPlayer;
    private bool eClicked = false;
    private double eDownTime = 0;
    private bool canPickUp = false; //turns true when e has been held long enough over the gremlin
    [HideInInspector] public bool beingPet = false;
    private bool beingCuddled = false;
    [HideInInspector] public bool beingCarried = false;
    private float petCooldownTimer = 0.0f; //timer for pet cooldown
    private float cuddleCooldownTimer = 0.0f; //timer for pet cooldown
    private float happinessDecayTimer = 0.0f;
    private bool enableStatMenu = false;
    private bool idleEntered = false; //flag to check if the idle state has been entered, used with the animator


    private Rigidbody rb;
    public Gremlin gremlin;

    public void Start()
    {

        player = GameObject.Find("Player");
        Canvas = GameObject.Find("Canvas (Hub UI)");
        StatMenu = Canvas.transform.Find("Stat Menu").gameObject;

        var interactions = Canvas.transform.Find("Interactions").gameObject;
        DropIndicator = interactions.transform.GetChild(0).gameObject;
        PickupIndicator = interactions.transform.GetChild(1).gameObject;
        PetIndicator = interactions.transform.GetChild(2).gameObject;
        CuddleIndicator = interactions.transform.GetChild(3).gameObject;
        TossIndicator = interactions.transform.GetChild(4).gameObject;
        StatIndicator = interactions.transform.GetChild(5).gameObject;

        CarriedGremlin = player.transform.GetChild(0).transform.Find("Carried Gremlin");
        CarriedFruit = player.transform.GetChild(0).transform.Find("Carried Fruit");

        PickupIndicator.SetActive(false);
        PetIndicator.SetActive(false);
        StatIndicator.SetActive(false);
        DropIndicator.SetActive(false);
        TossIndicator.SetActive(false);
        CuddleIndicator.SetActive(false);
        StatMenu.SetActive(false);
        rb = GetComponent<Rigidbody>();
        //GetComponent<Outline>().OutlineWidth = 0;

        gremlin = this.GetComponent<GremlinObject>().gremlin;
        audioController = this.GetComponentInChildren<GremlinAudioController>();
    }

    public void Update()
    {
        //distance between particular Gremlin and the player
        distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        var playerMove = player.GetComponent<PlayerMovement>();
        if (playerMove.centeredObject == this.gameObject)
            IsCentered();
        else if (playerMove.centeredObject != this.gameObject && playerMove.hitObjectIsNew)
            IsExited();

        if (beingCarried && !idleEntered) //switch the gremlin to idle if it is picked up
        {
            transform.Find("gremlinModel").GetComponent<Animator>().SetTrigger("isIdle");
            idleEntered = true;
        }
        if (!beingCarried) //switching the flag back if we put the gremlin down
            idleEntered = false;

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
                UpdateStats("Happiness", cuddleIncrease, maxHappinessVal);
            }

            //TOSS
            if (Input.GetMouseButtonDown(0) && !attemptYeet)
            {
                TossIndicator.SetActive(false);
                DropIndicator.SetActive(false);
                CuddleIndicator.SetActive(false);

                attemptYeet = true;

                audioController.PlayThrow();

                CarriedGremlin.GetComponent<Collider>().enabled = true;
                this.transform.parent = null;
                rb.constraints = RigidbodyConstraints.None;
                rb.useGravity = true;
                rb.AddForce(player.transform.GetChild(0).transform.forward * tossForce);
                GetComponent<Collider>().enabled = true;
            }
        }

        //pick up gremlin timer
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

        //happiness decay timer
        if (!beingCarried && !beingPet)
        {
            happinessDecayTimer += Time.deltaTime;
        }
        else
            happinessDecayTimer = 0.0f;
        if (happinessDecayTimer >= happinessDecayTime)
        {
            UpdateStats("Happiness", -happinessDecayAmount, maxHappinessVal);
            happinessDecayTimer = 0.0f;
        }

    }

    private void UpdateStats(string stat, float changeAmount, float maxVal)
    {
        float statChange = gremlin.getStat(stat) + changeAmount;
        if (Mathf.Abs(statChange) > maxVal)
            statChange = maxVal;
        gremlin.setStat(stat, statChange);
        UpdateStatsMenu();
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
            //GetComponent<Outline>().OutlineWidth = 10;
            //if (CarriedGremlin.childCount != 0 || beingPet || CarriedFruit.childCount != 0)
            //GetComponent<Outline>().OutlineWidth = 0;
            if (CarriedGremlin.childCount == 0 && !beingPet && CarriedFruit.childCount == 0 && !attemptYeet)
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
                UpdateStats("Happiness", petIncrease, maxHappinessVal);

                audioController.PlayPet();

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
                //GetComponent<Collider>().enabled = false;
                GetComponent<GremlinAI>().enabled = false;

                //have gremlin face the player
                transform.rotation = Quaternion.LookRotation(player.transform.position - this.transform.position, player.transform.up);

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
                UpdateStatsMenu();
            }


        }
        //not close enough to pick up
        else
        {
            onGremlin = false;
            PickupIndicator.SetActive(false);
            PetIndicator.SetActive(false);
            StatIndicator.SetActive(false);
            //GetComponent<Outline>().OutlineWidth = 0;
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

        //GetComponent<Outline>().OutlineWidth = 0;
        eDownTime = 0;
        eClicked = false;
        enableStatMenu = false;
        StatMenu.SetActive(enableStatMenu);
    }

    private void UpdateStatsMenu()
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
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        DropIndicator.SetActive(false);
        TossIndicator.SetActive(false);
        CuddleIndicator.SetActive(false);
        GetComponent<Collider>().enabled = true;
        GetComponent<GremlinAI>().enabled = true;
        GetComponent<GremlinAI>().move = false;
        eDownTime = 0;
        canPickUp = false;
    }

    /// Temporarily keeps carriedGremlin collider disabled for toss mechanic
    IEnumerator enableCarryCollider()
    {
        yield return new WaitForSeconds(0.5f);
        CarriedGremlin.GetComponent<Collider>().enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Carried Gremlin" && attemptYeet && rb.velocity.y < 0)
        {
            attemptYeet = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            this.transform.position = CarriedGremlin.position;
            this.transform.parent = CarriedGremlin;
            //GetComponent<Collider>().enabled = false;
            TossIndicator.SetActive(true);
            DropIndicator.SetActive(true);
            CuddleIndicator.SetActive(true);

            UpdateStats("Happiness", yeetIncrease, maxHappinessVal);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (beingCarried)
        {
            DropGremlin();
        }
        if (attemptYeet)
        {
            attemptYeet = false;
            UpdateStats("Happiness", -yeetIncrease, maxHappinessVal);
        }
    }

}