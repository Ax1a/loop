using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "InventoryItemsDB", menuName = "Inventory/Inventory items database")]
public class InventoryDatabase : ScriptableObject
{
    public InventoryItemData[] items;

   public int itemsCount
    {
        get
        {
            return items.Length;
        }
    }

   public InventoryItemData GetInventoryItems(int index)
    {
        return items[index];
    }
    
}
