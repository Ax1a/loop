using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Image invItemImage;
    [SerializeField] TMP_Text invItemName;
    [SerializeField] TMP_Text invItemEnergy;
    [SerializeField] TMP_Text invItemQuantity;

    [Space (20f)]
    [SerializeField] Button invItemConsumeBtn;

    public void SetInventoryItemPosition(Vector2 pos)
    {
        GetComponent <RectTransform> ().anchoredPosition += pos;
    }

    public void SetInventoryItemImg(Sprite sprite)
    {
        invItemImage.sprite = sprite;
    }

    public void SetInventoryItemName(string name)
    {
        invItemName.text = name;
    }

    public void SetInventoryItemEnergy(int energy)
    {
        invItemEnergy.text = energy.ToString();
    }

    public void SetInventoryItemQuantity(int quantity)
    {
        invItemQuantity.text = quantity.ToString();
    }


}
