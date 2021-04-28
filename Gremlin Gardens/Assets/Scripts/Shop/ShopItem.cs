using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    /// <summary>
    /// The name of the item (to display on text)
    /// </summary>
    [Tooltip("The name of the item (to display on text)")]
    public string itemName;
    /// <summary>
    /// What object to spawn when the player buys it.
    /// </summary>
    [Tooltip("What object to spawn when the player buys it.")]
    public GameObject itemSpawnOnBuy;
    /// <summary>
    /// If the player is intending to purchase the item.
    /// </summary>
    private bool purchaseIntent;
    /// <summary>
    /// If the mouse is over this current item.
    /// </summary>
    private bool mouseOn;

    /// <summary>
    /// The ShopManager that this is the child of.
    /// </summary>
    private ShopManager manager;

    /// <summary>
    /// The distance between the player and the shop item to show the buy text.
    /// </summary>
    public int buyDistance = 20;

    /// <summary>
    /// The distance between the player and the shop item to show the buy text.
    /// </summary>
    public int cost = 150;

    /// <summary>
    /// A reference to the UI Hover Sound
    /// </summary>
    public AudioSource hoverSound;

    /// <summary>
    /// A reference to the UI Shop Purchase Sound
    /// </summary>
    public AudioSource purchaseSound;

    /// <summary>
    /// A reference to the UI Shop Confirm Sound
    /// </summary>
    public AudioSource confirmSound;

    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        manager = this.GetComponentInParent<ShopManager>();
        hoverSound = GameObject.Find("UI Sounds").GetComponents<AudioSource>()[0];
        purchaseSound = GameObject.Find("UI Sounds").GetComponents<AudioSource>()[1];
        confirmSound = GameObject.Find("UI Sounds").GetComponents<AudioSource>()[2];
    }

    // Update is called once per frame
    void Update()
    {
        var playerMove = manager.player.GetComponent<PlayerMovement>();
        if (playerMove.centeredObject == this.gameObject && playerMove.hitObjectIsNew)
        {
            IsCentered();
        }
        else if (playerMove.centeredObject != this.gameObject && playerMove.hitObjectIsNew && playerMove.previousObject == this.gameObject)
        {
            IsExited();
        }

        if (Vector3.Distance(manager.player.transform.position, this.transform.position) < buyDistance && mouseOn)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == false)
            {
                purchaseIntent = true;
                manager.SetPurchaseText("Confirm Buy " + itemName + "?");
                confirmSound.Play();
            } else if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == true) {
                purchaseIntent = false;
                

                if (manager.player.GetComponent<PlayerMovement>().UpdateMoney(-150))
                {
                    var bought = Instantiate(itemSpawnOnBuy);
                    bought.transform.position = manager.player.transform.position + manager.player.transform.forward;
                    manager.SetPurchaseText("Buy " + itemName + "?");
                    purchaseSound.Play();
                } else
                {
                    manager.SetPurchaseText($"Sorry player, you need {150 - manager.player.GetComponent<PlayerMovement>().GetMoney()} more money");
                }
                //Temporary solution for placement.
            }
        }
    }

    // used for shopping as well
    private void IsCentered()
    {
        mouseOn = true;
        manager.ItemHover(this);
        manager.SetPurchaseText("Buy " + itemName + "?\nCost: " + cost);
        hoverSound.Play();

    }

    private void IsExited()
    {
        manager.ItemExit();
        mouseOn = false;
        purchaseIntent = false;
    }
}