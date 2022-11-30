using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopScript : MonoBehaviour
{

    [Header ("Shop Events")]
    [SerializeField] GameObject shopUI;
    [SerializeField] Button openShopBtn;
    [SerializeField] Button closeShopBtn;

    // Start is called before the first frame update
    void Start()
    {
        AddShopEvents();
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
