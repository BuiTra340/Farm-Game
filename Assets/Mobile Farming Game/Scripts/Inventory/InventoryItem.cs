using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem 
{
    public CropType cropType;
    public int amount;
    public InventoryItem(CropType _cropType,int _amount)
    {
        cropType = _cropType;
        amount = _amount;
    }
}
