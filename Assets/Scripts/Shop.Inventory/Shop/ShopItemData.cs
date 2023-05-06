using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItemData
{
    public Sprite itemImg;
    public string itemName;
    [Range (1,15)] public int itemEnergy;
    public int price;
    public ItemType itemType;
    public int levelRequirement;
    public enum ItemType { Drink, Food }
    public bool isPurchased;

}

