using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBuying : MonoBehaviour
{
	public static Dictionary<string, Vector3> TextPositions = new Dictionary<string, Vector3>();

    /*public GameObject ItemPurchaseIndicator;  	  //button prompt to purchase item.
	public GameObject ConfirmPurchaseIndicator;	  //button prompt to confirm purchase.
    public GameObject CancelPurchaseIndicator;    //button prompt to drop down*/
    public GameObject PurchaseText; // Text that appears above the fruit
    public GameObject player;
    
    [SerializeField]
    [Tooltip("How far above the center of the food the text will appear")]
    private float YTextOffset = 0.4f;

    private Food thisFood; // The Food object correlating to the on-counter item
    private float distanceFromPlayer; //distance (in meters?) from player to fruit
    private bool onFruit; //is mouse currently over the fruit
    public bool purchaseIntent; //if player has pressed ItemPurchaseIndic key.
    private bool inShop = true;		//checks whether playeere entered/exited shop.
    public bool mouseOn;
	//create a viewdFruit object

    void Awake()
    {
        Vector3 selfPosition = this.transform.position;
        TextPositions.Add(name, new Vector3(selfPosition.x, selfPosition.y + YTextOffset, selfPosition.z));
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseOn = false;
        PurchaseText.SetActive(false);
        /*ItemPurchaseIndicator.SetActive(false);
        CancelPurchaseIndicator.SetActive(false);
        ConfirmPurchaseIndicator.SetActive(false);*/
    }

    // Update is called once per frame
    void Update()
    {
    	distanceFromPlayer = Vector3.Distance(player.transform.position, this.transform.position);
        if (distanceFromPlayer < 20 && inShop && mouseOn)
        {
            PurchaseText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == false)
            {
                purchaseIntent = true;
                PurchaseText.GetComponent<TextMesh>().text = "Confirm: Buy " + this.name + "?";
            } else if (Input.GetKeyDown(KeyCode.Mouse0) && purchaseIntent == true) {
                PurchaseText.SetActive(false);
                PurchaseText.GetComponent<TextMesh>().text = "Click To Buy " + name;
                purchaseIntent = false;
            }
            /*ItemPurchaseIndicator.SetActive(true);
            //freeze camera if input Purchase?
            if (Input.GetKeyDown("u"))
            {
                purchaseIntent = true;
                ConfirmPurchaseIndicator.SetActive(true);
                CancelPurchaseIndicator.SetActive(true);
            }
            if (purchaseIntent)
            {
                if (Input.GetKeyDown("i"))
                {
                    ConfirmPurchaseIndicator.SetActive(false);
                    CancelPurchaseIndicator.SetActive(false);
                    purchaseIntent = false;
                    //unfreeze cam n go to gremstork animation
                }
                else if (Input.GetKeyDown("o"))
                {
                    //unfreeze cam
                    ConfirmPurchaseIndicator.SetActive(false);
                    CancelPurchaseIndicator.SetActive(false);
                    purchaseIntent = false;
                }
            }*/
        }
    }

    /* void FixedUpdate() {
    	transform.Rotate(0, 1, 0);
    }*/

    // Centers PurchaseText over this
    private void centerText()
    {
        PurchaseText.transform.position = TextPositions[this.name];
    }

    // used for shopping as well
    private void OnMouseEnter()
    {
        PurchaseText.GetComponent<TextMesh>().text = "Click To Buy " + name;
        centerText();
        mouseOn = true;
    }

    private void OnMouseExit()
    {
        mouseOn = false;
        purchaseIntent = false;
        PurchaseText.SetActive(false);
        /*ItemPurchaseIndicator.SetActive(false);
        ConfirmPurchaseIndicator.SetActive(false);
        CancelPurchaseIndicator.SetActive(false);*/
    }
}