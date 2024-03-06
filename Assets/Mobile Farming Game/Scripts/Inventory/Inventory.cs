using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    [SerializeField]private List<InventoryItem> items = new List<InventoryItem>();
    public void cropHarvestedCallback(CropType cropType)
    {
        bool cropFound = false;
        for(int i=0;i<items.Count;i++)
        {
            InventoryItem item = items[i];
            if(item.cropType == cropType)
            {
                item.amount++;
                cropFound = true;
                break;
            }
        }
        //debugInventory();
        if (cropFound)
            return;
        items.Add(new InventoryItem(cropType, 1));
    }
    public InventoryItem[] GetInventoryItems() => items.ToArray();
    private void debugInventory()
    {
        foreach(var item in items)
            Debug.Log("have " + item.amount + " " + item.cropType +" in list! ");
    }
    public void Clear() => items.Clear();
}
