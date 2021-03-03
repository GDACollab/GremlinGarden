using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> itemsToInstantiate;

    [HideInInspector]
    public List<ShopItem> shopItems;

    public GameObject player;

    public Vector3 itemStartingPos;

    public Vector3 offsetDistance;

    public GameObject PurchaseText;

    Vector3 shopItemOffset;

    // Start is called before the first frame update
    void Start()
    {
        shopItemOffset = Vector3.zero;
        for (int i = 0; i < itemsToInstantiate.Count; i++) {
            AddItem(itemsToInstantiate[i]);
        }
    }

    public GameObject AddItem(GameObject prefab) {
        var newItem = Instantiate(prefab, this.transform);
        newItem.transform.position = itemStartingPos + shopItemOffset;
        shopItems.Add(newItem.GetComponent<ShopItem>());
        shopItemOffset += offsetDistance;
        return newItem;
    }

    public void RemoveItem(int index) {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (i == index)
            {
                shopItems.Remove(shopItems[i]);
                Destroy(shopItems[i].gameObject);
            }
        }
    }
    public void RemoveItem(GameObject itemToRemove) {
        for (int i = 0; i < shopItems.Count; i++) {
            if (shopItems[i].gameObject == itemToRemove) {
                shopItems.Remove(shopItems[i]);
                Destroy(shopItems[i].gameObject);
            }
        }
    }

    public void ItemHover(ShopItem item) {
        PurchaseText.SetActive(true);
        PurchaseText.transform.position = item.transform.position + new Vector3(0, 0.5f, 0);
    }

    public void SetPurchaseText(string text) {
        PurchaseText.GetComponent<TextMesh>().text = text;
    }

    public void ItemExit() {
        PurchaseText.SetActive(false);
    }
}
