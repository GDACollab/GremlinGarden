using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBuying : MonoBehaviour
{
	public GameObject ItemPurchaseIndicator;  	  //button prompt to purchase item.
	public GameObject ConfirmPurchaseIndicator;	  //button prompt to confirm purchase.
    public GameObject CancelPurchaseIndicator;    //button prompt to drop down
    public GameObject player;

    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    private bool purchaseIntent; //if player has pressed ItemPurchaseIndic key.
    private bool inShop = true;		//checks whether playeere entered/exited shop.
	//create a viewdFruit object

    // Start is called before the first frame update
    void Start()
    {
        ItemPurchaseIndicator.SetActive(false);
        CancelPurchaseIndicator.SetActive(false);
        ConfirmPurchaseIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    	distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
    }

    /* void FixedUpdate() {
    	transform.Rotate(0, 1, 0);
    }*/

    // used for shopping as well
    private void OnMouseEnter()
    {
        if (distanceFromPlayer < 20 && inShop)
        {
            GetComponent<Outline>().OutlineWidth = 5;
            ItemPurchaseIndicator.SetActive(true);
            //freeze camera if input Purchase?
            if(Input.GetKeyDown("U")) {
            	purchaseIntent = true;
            	ConfirmPurchaseIndicator.SetActive(true);
            	CancelPurchaseIndicator.SetActive(true);
            }
            if(purchaseIntent) {
            	if(Input.GetKeyDown("I")) {
            		ConfirmPurchaseIndicator.SetActive(false);
            		CancelPurchaseIndicator.SetActive(false);
            		purchaseIntent = false;
            		GetComponent<Outline>().OutlineWidth = 0;
            		//unfreeze cam n go to gremstork animation
            	} else if(Input.GetKeyDown("O")) {
            		//unfreeze cam
            		ConfirmPurchaseIndicator.SetActive(false);
            		CancelPurchaseIndicator.SetActive(false);
            		purchaseIntent = false;
            		GetComponent<Outline>().OutlineWidth = 0;
            	}
            }
        }
    }

    private void OnMouseExit()
    {
        purchaseIntent = false;
        ItemPurchaseIndicator.SetActive(false);

        GetComponent<Outline>().OutlineWidth = 0;
    }
}