using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ShopItemsDatabase", menuName = "Shop/Items shop database")]
public class ShopItemsDatabase : ScriptableObject
{
   public ShopItemData[] items;

   public int itemsCount
    {
        get
        {
            return items.Length;
        }
    }

   public ShopItemData GetItems(int index)
    {
        return items[index];
    }

   public void PurchaseItem(int index)
    {
        items[index].isPurchased = true;
    }
}
