using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerToolSelector : MonoBehaviour
{
    public enum Tool { None,Sow,Water,Harvest}
    private Tool activeTool;
    [Header("Elements")]
    [SerializeField] private Image[] toolImages;
    [Header("Settings")]
    [SerializeField] private Color toolSelectedColor;

    [Header("Action")]
    public Action<Tool> onToolSelected;
    private void Start()
    {
        selectTool(0);
    }
    public void selectTool(int toolIndex)
    {
        activeTool = (Tool)toolIndex;
        for(int i=0;i<toolImages.Length;i++)
        {
            toolImages[i].color = i == toolIndex ? toolSelectedColor : Color.white;
        }
        onToolSelected?.Invoke(activeTool);
    }
    public bool canSow() => activeTool == Tool.Sow;
    public bool canWater() => activeTool == Tool.Water;
    public bool canHarvest()=> activeTool == Tool.Harvest;

}
