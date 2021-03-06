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

    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        manager = this.GetComponentInParent<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(manager.player.transform.position, this.transform.position) < buyDistance && mouseOn)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == false)
            {
                purchaseIntent = true;
                manager.SetPurchaseText("Confirm Buy " + itemName + "?");
            } else if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == true) {
                purchaseIntent = false;
                var bought = Instantiate(itemSpawnOnBuy);
                //Temporary solution for placement.
                bought.transform.position = manager.player.transform.position + manager.player.transform.forward;
                manager.SetPurchaseText("Buy " + itemName + "?");
            }
        }
    }

    // used for shopping as well
    private void OnMouseEnter()
    {
        mouseOn = true;
        manager.ItemHover(this);
        manager.SetPurchaseText("Buy " + itemName + "?");
        Debug.Log("e");
    }

    private void OnMouseExit()
    {
        manager.ItemExit();
        mouseOn = false;
        purchaseIntent = false;
    }
}