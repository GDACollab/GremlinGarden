using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
	public GameObject ItemPurchaseIndicator;  	  //button prompt to purchase item.
	public GameObject ConfirmPurchaseIndicator;	  //button prompt to confirm purchase.
    public GameObject CancelPurchaseIndicator;    //button prompt to drop down
    public GameObject player;

    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    private bool purchaseIntent; //if player has pressed ItemPurchaseIndic key.
	//create a viewdFruit object

    // Start is called before the first frame update
    void Start()
    {
        ItemPurchaseIndicator.SetActive(false);
        CancelPurchaseIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    	//if Input.GetKeyDown("***")
    		//show Item.Cost
    		//show ConfirmButtonIndic.

     //calculate distance between each fruit somehow. use foreach?

    }

    // used for shopping as well
    private void OnMouseOver()
    {
        if (distanceFromPlayer < 1)
        {
            onFruit = true;
            GetComponent<Outline>().OutlineWidth = 5;
            ItemPurchaseIndicator.SetActive(true);

            //if no fruit is being looked at already
            if (selectedFruit.childCount == 0)	{
                //remove gravity so object isnt spazzing out
                
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

class Items {
	public Dictionary<GameObject, int> item_costs = new Dictionary<GameObject, int>{
		{GameObject.Find("Sphere"), 20},
		{GameObject.Find("Sphere (1)"), 25},
		{GameObject.Find("Sphere (2)"), 30},
		{GameObject.Find("Sphere (3)"), 30},
		{GameObject.Find("Sphere (4)"), 40},
		{GameObject.Find("Sphere (5)"), 50}
	};

	// If item purchased
	public getItem(GameObject selectedItem) {
		if ()
	}
}