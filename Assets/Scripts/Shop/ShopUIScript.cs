using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIScript : MonoBehaviour
{

    [Header ("Shop Layout Settings")]
    [SerializeField] float itemSpacing = 1f;
    float itemHeight;

    [Header ("Shop UI Elements")]
    [SerializeField] Transform ShopMenu;
    [SerializeField] Transform ShopItemsContainer;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] TextMeshProUGUI insufficientFunds;
    [SerializeField] TextMeshProUGUI reduceFunds;
    [SerializeField] GameObject placeholderItem;
    [Space (20)]
    [SerializeField] ShopItemsDatabase itemsDB;

    private Coroutine showCoroutineReduce, showCoroutineInsufficient;

    // Start is called before the first frame update
    void Start()
    {
        GenerateShopItemsUI();
    }

    //generates the shop items through the database
    void GenerateShopItemsUI()
    {
        itemHeight = ShopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(ShopItemsContainer.GetChild(0).gameObject);
        ShopItemsContainer.DetachChildren();

        for(int x = 0; x < itemsDB.itemsCount; x++)
        {
            int index = x;
            ShopItemData item = itemsDB.GetItems(x);
            ShopItemsUI uiItem = Instantiate(itemPrefab, ShopItemsContainer).GetComponent<ShopItemsUI>();

            // Add onclick listener per items on loop
            uiItem.GetComponentInChildren<Button>().onClick.AddListener(delegate {BuyItem(item);});   
            
            //sets the position of generated items based on parent
            uiItem.SetItemPosition(Vector2.right * x * (itemHeight + itemSpacing));

            uiItem.SetItemImg(item.itemImg);
            uiItem.SetItemName(item.itemName);
            uiItem.SetItemEnergy(item.itemEnergy);
            uiItem.SetItemPrice(item.price);
        }

        for(int j = 0; j < 24 - itemsDB.itemsCount; j++) {
            Instantiate(placeholderItem, ShopItemsContainer);
        }
    }

    // Onclick Function - Add function for storing inventory
    public Inventory inventory;

    void BuyItem(ShopItemData item) 
    {
        if(DataManager.CanSpendMoney(item.price)) {
            insufficientFunds.gameObject.SetActive(false);

            DataManager.AddInventoryItem(item);
            DataManager.SpendMoney(item.price);
            
            if (showCoroutineReduce != null) StopCoroutine(showCoroutineReduce);
            showCoroutineReduce = StartCoroutine(ShowReduceFunds(item.price));
            
            if (DataManager.GetQuestProgress() == 1) {
                BotGuide.Instance.AddDialogue("Great! You can grab a food or beverage when you need an energy."); 
                BotGuide.Instance.AddDialogue("Now let's close the shop to continue the tutorial.");
                BotGuide.Instance.ShowDialogue();
                UIController.Instance.SetHighlightActive(false);
            }
        }
        else {
            if (showCoroutineInsufficient != null) StopCoroutine(showCoroutineInsufficient);
            showCoroutineInsufficient = StartCoroutine(ShowInsufficientText());
        }
    }

    private void OnDisable() {
        insufficientFunds.gameObject.SetActive(false);
        reduceFunds.gameObject.SetActive(false);
    }

    IEnumerator ShowInsufficientText() {
        insufficientFunds.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        insufficientFunds.gameObject.SetActive(false);
    }

    IEnumerator ShowReduceFunds(int price) {
        reduceFunds.gameObject.SetActive(true);
        reduceFunds.text = "-" + price.ToString();
        yield return new WaitForSeconds(1.5f);
        reduceFunds.gameObject.SetActive(false);
    }
}
