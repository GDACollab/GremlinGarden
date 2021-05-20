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
    /// How much the item costs.
    /// </summary>
    [Tooltip("How much the item costs.")]
    public int cost = 100;

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

    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        manager = this.GetComponentInParent<ShopManager>();
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
            // enableMovement is to make sure the player is not in a menu or something when clicking
            PlayerMovement playerMovement = manager.player.GetComponent<PlayerMovement>();
            if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == false && playerMovement.enableMovement && playerMovement.GetMoney() >= cost)
            {
                purchaseIntent = true;
                manager.SetPurchaseText("Confirm Buy " + itemName + "?");
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == true && playerMovement.enableMovement)
            {
                purchaseIntent = false;
                // Quick hack to detect whether or not we're spawning a gremlin.
                if (itemSpawnOnBuy.TryGetComponent<GremlinObject>(out GremlinObject gremlin))
                {
                    // Quick hack to find the GameManager:
                    GameObject.Find("GameManager").GetComponent<GremlinSpawner>().CreateGremlin(manager.player.transform.position + manager.player.transform.forward);
                }
                else
                {
                    var bought = Instantiate(itemSpawnOnBuy);
                    //Temporary solution for placement.
                    bought.transform.position = manager.player.transform.position + manager.player.transform.forward;
                }
                playerMovement.UpdateMoney(-cost);
                manager.SetPurchaseText("Buy " + itemName + "?");
            }
            else {
                manager.SetPurchaseText("Insufficient Funds to Buy " + itemName + ".");
            }
        }
    }

    // used for shopping as well
    private void IsCentered()
    {
        mouseOn = true;
        manager.ItemHover(this);
        manager.SetPurchaseText("Buy " + itemName + "?");
    }

    private void IsExited()
    {
        manager.ItemExit();
        mouseOn = false;
        purchaseIntent = false;
    }
}