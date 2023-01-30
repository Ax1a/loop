using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Inventory : MonoBehaviour
{
    //list for inventory to store items
    public List<InventoryItemData> inventoryItems;

     // Prefab for the list item
    public GameObject listItemPrefab;
    public GameObject mainUI;
    public GameObject inventoryUI;

    // Parent object for the list items
    public Transform listItemParent;

    [SerializeField] float itemSpacing = 1f;
    float itemHeight;

    public void AddItem(ShopItemData item)
    {

        InventoryItemData inventoryItem = new InventoryItemData();

        // Assign the values from the ShopItemData object to the fields of the InventoryItemData object
        inventoryItem.itemImg = item.itemImg;
        inventoryItem.itemName = item.itemName;
        inventoryItem.itemEnergy = item.itemEnergy;

        // Add the InventoryItemData object to the inventory
        inventoryItems.Add(inventoryItem);
    }


    public int inventoryItemsCount
    {
        get
        {
            return inventoryItems.Count;
        }
    }
    //public 

    //populating inventory UI


    void OnEnable()
    {
        PopulateListUI();
    }
  
    //doesnt work?

    public void PopulateListUI()
    {
    // Instantiate a game object for each item in the list and add it to the list UI
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItemData invItem = inventoryItems[i];
            GameObject listItem = Instantiate(listItemPrefab, listItemParent);
            
            // Set the values for the list item
            listItem.GetComponent<InventoryUI>().SetInventoryItemImg(invItem.itemImg);
            listItem.GetComponent<InventoryUI>().SetInventoryItemName(invItem.itemName);
            listItem.GetComponent<InventoryUI>().SetInventoryItemEnergy(invItem.itemEnergy);
        }
        // Clear the list UI
        Transform parent = GameObject.Find("InventoryItems").transform;

        if(parent.childCount != 0) GameObject.Destroy(parent.GetChild(0).gameObject);
    }

    public void closeInventory() {
        gameObject.SetActive(false);
        mainUI.SetActive(true);
    }
}
