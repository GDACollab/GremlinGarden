using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{

    public string itemName;
    public GameObject itemSpawnOnBuy;
    private bool purchaseIntent; //if player has pressed ItemPurchaseIndic key.
    private bool mouseOn;

    private ShopManager manager;

    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        manager = this.GetComponentInParent<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(manager.player.transform.position, this.transform.position) < 10 && mouseOn)
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
    }

    private void OnMouseExit()
    {
        manager.ItemExit();
        mouseOn = false;
        purchaseIntent = false;
    }
}