using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropTile : MonoBehaviour
{
    private TileFieldState state;
    [Header("Elements")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    private CropData cropData;

    [Header("Actions")]
    public static Action<CropType> onCropHarvested;
    private void Start()
    {
        state = TileFieldState.Empty;
    }
    public void Sow(CropData cropData)
    {
        state = TileFieldState.Sown;
        crop = Instantiate(cropData.cropPrefab,transform.position,Quaternion.identity, cropParent);
        this.cropData = cropData;
    }
    public void Water()
    {
        state = TileFieldState.Watered;
        tileRenderer.gameObject.LeanColor(Color.white * .3f, 1).setEase(LeanTweenType.easeOutBack);
        crop.ScaleUp();
    }
    public bool isEmpty()
    {
        return state == TileFieldState.Empty;
    }
    public bool isSown() => state == TileFieldState.Sown;
    public bool isWatered() => state == TileFieldState.Watered;

    public void Harvest()
    {
        state = TileFieldState.Empty;
        crop.scaleDown();
        tileRenderer.gameObject.LeanColor(Color.white, 1).setEase(LeanTweenType.easeOutBack);
        onCropHarvested?.Invoke(cropData.cropType);
    }
}
