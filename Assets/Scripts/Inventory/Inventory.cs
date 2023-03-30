using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class Inventory : MonoBehaviour
{
    [Header ("Inventory UI Elements")]
    [SerializeField] private GameObject placeholder;
    public GameObject mainUI;
    public GameObject inventoryUI;
    [SerializeField] private TextMeshProUGUI fullEnergyTxt;
    [SerializeField] private TextMeshProUGUI addEnergyTxt;
     // Prefab for the list item
    public GameObject listItemPrefab;
    // Parent object for the list items
    public Transform listItemParent;

    [Header ("Instances")]
    [SerializeField] private ShopItemsDatabase shopDB;
    float itemHeight;
    Coroutine showFullEnergyCoroutine, showAddEnergyCoroutine;

    private void OnEnable() {
        PopulateListUI();
    }

    // Populating inventory UI
    public void PopulateListUI()
    {
        if (listItemParent.childCount != 0) {
            foreach (Transform item in listItemParent.transform)
            {
                GameObject.Destroy(item.gameObject);
            }
        }
        // Display all the inventory items based on the item name
        for(int i = 0; i < DataManager.GetInventoryListCount(); i++)
        {
            // Loop through shop item database
            foreach (var item in shopDB.items)
            {
                if (item.itemName == DataManager.GetInventoryItemName(i)) {
                    GameObject listItem = Instantiate(listItemPrefab, listItemParent);
                    // Get the parent GameObject
                    Transform parentObject = listItem.transform;
                    // Set the values for the list item
                    listItem.GetComponent<InventoryUI>().SetInventoryItemImg(item.itemImg);
                    listItem.GetComponent<InventoryUI>().SetInventoryItemName(item.itemName);
                    listItem.GetComponent<InventoryUI>().SetInventoryItemEnergy(item.itemEnergy);
                    listItem.GetComponent<InventoryUI>().SetInventoryItemQuantity(DataManager.GetInventoryItemQuantity(i));
                    listItem.GetComponentInChildren<Button>().onClick.AddListener(delegate {UseItem(item.itemName, parentObject);});   
                }
            }
        }

        for(int j = 0; j < 24 - DataManager.GetInventoryListCount(); j++) {
            Instantiate(placeholder, listItemParent);
        }
    }

    public void UseItem(string name, Transform parent) {
        if (Energy.Instance.GetCurrentEnergy() >= Energy.Instance.maxEnergy) {
            if (showFullEnergyCoroutine != null) StopCoroutine(showFullEnergyCoroutine);

            showFullEnergyCoroutine = StartCoroutine(ShowFullEnergyTxt());
        }
        else {
            foreach (var item in DataManager.GetInventoryList())
            {
                if(name == item.name) {
                    DataManager.ReduceItemQuantity(name);
                    
                    Energy.Instance.SetCurrentEnergy(item.energy + Energy.Instance.GetCurrentEnergy());
                    Energy.Instance.UpdateEnergyTime();
                    // Update UI of item
                    parent.GetComponent<InventoryUI>().SetInventoryItemQuantity(item.quantity);

                    // Display added energy
                    if (showAddEnergyCoroutine != null) StopCoroutine(showAddEnergyCoroutine);

                    showAddEnergyCoroutine = StartCoroutine(ShowAddEnergyTxt(item.energy));

                    if (item.quantity == 0) {
                        DataManager.RemoveInventoryItem(name);
                        Destroy(parent.gameObject);
                        Instantiate(placeholder, listItemParent);
                        break;
                    }
                }
            }
        }
    }

    private void OnDisable() {
        fullEnergyTxt.gameObject.SetActive(false);
        addEnergyTxt.gameObject.SetActive(false);
    }

    IEnumerator ShowFullEnergyTxt() {
        fullEnergyTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        fullEnergyTxt.gameObject.SetActive(false);
    }

    IEnumerator ShowAddEnergyTxt(int energy) {
        addEnergyTxt.gameObject.SetActive(true);
        addEnergyTxt.text = "+ " + energy.ToString();
        yield return new WaitForSeconds(1.5f);
        addEnergyTxt.gameObject.SetActive(false);
    }

    public void closeInventory() {
        gameObject.SetActive(false);
        mainUI.SetActive(true);
    }
}
