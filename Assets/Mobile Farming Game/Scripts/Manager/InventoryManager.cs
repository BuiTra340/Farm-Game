using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private Inventory inventory;
    private string dataPath;
    private InventoryDisplay inventoryDisplay;
    private void Start()
    {
        dataPath = Application.persistentDataPath + "/buitra.data";
        loadInventory();
        configureInventoryDisplay();
        CropTile.onCropHarvested += cropHarvestedCallback;
        Apple.onAppleReleased += cropHarvestedCallback;
    }

    private void configureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
        inventoryDisplay.updateDisplay(inventory);
    }

    private void OnDestroy()
    {
        CropTile.onCropHarvested -= cropHarvestedCallback;
        Apple.onAppleReleased -= cropHarvestedCallback;
    }
    private void cropHarvestedCallback(CropType cropType)
    {
        inventory.cropHarvestedCallback(cropType);
        inventoryDisplay.updateDisplay(inventory);
        saveInventory();
    }
    [NaughtyAttributes.Button]
    public void clearInventory()
    {
        inventory.Clear();
        inventoryDisplay.updateDisplay(inventory);
        saveInventory();
    }
    public Inventory getInventory() => inventory;
    private void saveInventory()
    {
        string data = JsonUtility.ToJson(inventory);
        File.WriteAllText(dataPath, data);
    }
    private void loadInventory()
    {
        string data = "";
        if(File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);

            if(inventory == null)
                inventory = new Inventory();
        }else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }
    }
}
