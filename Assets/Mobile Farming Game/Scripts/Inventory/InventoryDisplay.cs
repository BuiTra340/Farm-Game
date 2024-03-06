using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropContainerParent;
    [SerializeField] private UICropContainer cropContainerPrefab;
    public void Configure(Inventory inventory)
    {
        foreach(var item in inventory.GetInventoryItems())
        {
            UICropContainer cropContainer = Instantiate(cropContainerPrefab,cropContainerParent);
            Sprite icon = DataManager.instance.getCropIcon(item.cropType);
            cropContainer.configure(icon, item.amount);
        }
    }
    public void updateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();
        for(int i=0;i<items.Length;i++)
        {
            if(i < cropContainerParent.childCount)
            {
                Sprite icon = DataManager.instance.getCropIcon(items[i].cropType);
                cropContainerParent.GetChild(i).GetComponent<UICropContainer>().configure(icon, items[i].amount);
                cropContainerParent.GetChild(i).gameObject.SetActive(true);
            }else
            {
                UICropContainer cropContainer = Instantiate(cropContainerPrefab, cropContainerParent);
                Sprite icon = DataManager.instance.getCropIcon(items[i].cropType);
                cropContainer.configure(icon,items[i].amount);
            }
        }

        int remainingContainers = cropContainerParent.childCount - items.Length;
        if (remainingContainers <= 0)
            return;
        for (int i = 0; i < remainingContainers; i++)
            cropContainerParent.GetChild(items.Length + i).gameObject.SetActive(false);
    }
}
