using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIScript : MonoBehaviour
{

    [Header ("Shop Layout Settings")]
    [SerializeField] float itemSpacing = .5f;
    float itemHeight;

    [Header ("Shop UI Elements")]
    [SerializeField] Transform ShopMenu;
    [SerializeField] Transform ShopItemsContainer;
    [SerializeField] GameObject itemPrefab;
    [Space (20)]
    [SerializeField] ShopItemsDatabase itemsDB;

    [Header ("Shop Events")]
    [SerializeField] GameObject shopUI;
    [SerializeField] Button openShopBtn;
    [SerializeField] Button closeShopBtn;

    // Start is called before the first frame update
    void Start()
    {
        AddShopEvents();
        GenerateShopItemsUI();
    }

    void GenerateShopItemsUI()
    {
        itemHeight = ShopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(ShopItemsContainer.GetChild(0).gameObject);
        ShopItemsContainer.DetachChildren();

        for(int x = 0; x < itemsDB.itemsCount; x++)
        {
            ShopItemData item = itemsDB.GetItems(x);
            ShopItemsUI uiItem = Instantiate(itemPrefab, ShopItemsContainer).GetComponent<ShopItemsUI>();

             uiItem.SetItemPosition(Vector2.right * x * (itemHeight + itemSpacing));

             uiItem.SetItemImg(item.itemImg);
             uiItem.SetItemName(item.itemName);
             uiItem.SetItemEnergy(item.itemEnergy);
             uiItem.SetItemPrice(item.price);
        }
    }

    void AddShopEvents()
    {
        openShopBtn.onClick.RemoveAllListeners();
        openShopBtn.onClick.AddListener(openShop);

        closeShopBtn.onClick.RemoveAllListeners();
        closeShopBtn.onClick.AddListener(closeShop);
    }

    void openShop()
    {
        shopUI.SetActive(true);
    }

    void closeShop()
    {
        shopUI.SetActive(false);
    }
}
