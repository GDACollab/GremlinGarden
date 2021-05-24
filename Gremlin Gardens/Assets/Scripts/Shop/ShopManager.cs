using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    /// <summary>
    /// A list of items with which to Instantiate the shop with.
    /// </summary>
    public List<GameObject> itemsToInstantiate;

    /// <summary>
    /// The items that the ShopManager is keeping track of.
    /// </summary>
    [HideInInspector]
    public List<ShopItem> shopItems;

    /// <summary>
    /// The player that the shop manager is keeping track of.
    /// </summary>
    [Tooltip("The player that the shop manager is keeping track of.")]
    public GameObject player;

    /// <summary>
    /// The starting position where the ShopManager should place the items (use an empty game object's position and .position.right (the red arrow) for placement direction)
    /// </summary>
    [Tooltip("The starting position where the ShopManager should place the items (use an empty game object's position and .position.right (the red arrow) for placement direction)")]
    public GameObject itemStartingPos;

    public float offsetDistance = 2.0f;

    /// <summary>
    /// The PurchaseText object to use for purchasing text.
    /// </summary>
    [Tooltip("The PurchaseText object to use for purchasing text.")]
    public GameObject PurchaseText;

    /// <summary>
    /// The current offset for shop items.
    /// </summary>
    Vector3 shopItemOffset;

    // Start is called before the first frame update
    void Start()
    {
        shopItemOffset = Vector3.zero;
        for (int i = 0; i < itemsToInstantiate.Count; i++) {
            AddItem(itemsToInstantiate[i]);
        }
    }

    /// <summary>
    /// Add an item to the ShopManager.
    /// </summary>
    /// <param name="prefab">The prefab to instantiate (must have ShopItem script)</param>
    /// <returns></returns>
    public GameObject AddItem(GameObject prefab) {
        var newItem = Instantiate(prefab, this.transform);
        newItem.transform.position = itemStartingPos.transform.position + shopItemOffset;
        shopItems.Add(newItem.GetComponent<ShopItem>());
        shopItemOffset += itemStartingPos.transform.right * offsetDistance;
        return newItem;
    }

    /// <summary>
    /// Remove an item from the ShopManager.
    /// </summary>
    /// <param name="index">The index of the item.</param>
    public void RemoveItem(int index) {
        shopItems.Remove(shopItems[index]);
        Destroy(shopItems[index].gameObject);
    }

    /// <summary>
    /// Remove a GameObject to remove from the ShopManager's list of shopItems.
    /// </summary>
    /// <param name="itemToRemove">The GameObject to remove</param>
    public void RemoveItem(GameObject itemToRemove) {
        for (int i = 0; i < shopItems.Count; i++) {
            if (shopItems[i].gameObject == itemToRemove) {
                shopItems.Remove(shopItems[i]);
                Destroy(shopItems[i].gameObject);
            }
        }
    }

    /// <summary>
    /// Called by ShopItem, lets the ShopManager know to hover the purchase text over the given item.
    /// </summary>
    /// <param name="item">The ShopItem that called this function.</param>
    public void ItemHover(ShopItem item) {
        PurchaseText.SetActive(true);
        PurchaseText.transform.position = item.transform.position + new Vector3(0, 0.5f, 0);
    }

    /// <summary>
    /// Sets PurchaseText to a given string.
    /// </summary>
    /// <param name="text">The text to set.</param>
    public void SetPurchaseText(string text) {
        PurchaseText.GetComponent<TextMesh>().text = text;
    }

    /// <summary>
    /// Called by ShopItem, Disables PurchaseText
    /// </summary>
    public void ItemExit() {
        PurchaseText.SetActive(false);
    }
}
