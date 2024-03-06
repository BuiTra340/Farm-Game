using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop Data",menuName ="Scriptable Object/CropData", order = 0)]
public class CropData : ScriptableObject
{
    [Header("Settings")]
    public Crop cropPrefab;
    public CropType cropType;
    public Sprite icon;
    public int price;
}
