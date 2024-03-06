using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("Elements")]
    [SerializeField] private CropData[] cropData;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else Destroy(instance.gameObject);
    }
    public Sprite getCropIcon(CropType _cropType)
    {
        for (int i = 0; i < cropData.Length; i++)
            if (cropData[i].cropType == _cropType)
                return cropData[i].icon;

        Debug.Log("Not found icon Crop");
        return null;
    }
    public int getCropPriceFromCropType(CropType _cropType)
    {
        for(int i = 0;i < cropData.Length;i++)
            if (cropData[i].cropType == _cropType)
                return cropData[i].price;
        return 0;
    }
}
