using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ShopItemsUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemEnergy;
    [SerializeField] TMP_Text itemPrice;

    [Space (20f)]
    [SerializeField] Button itemPurchaseBtn;

    public void SetItemPosition(Vector2 pos)
    {
        GetComponent <RectTransform> ().anchoredPosition += pos;
    }

    public void SetCharacterImg(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }

    public void SetItemName(string name)
    {
        itemName.text = name;
    }

    public void SetItemEnergy(string energy)
    {
        itemEnergy.text = energy;
    }

    public void SetItemPrice(string price)
    {
        itemPrice.text = price;
    }
}
