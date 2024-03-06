using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropField : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform tilesParent;
    private List<CropTile> cropTiles = new List<CropTile>();

    [Header("Settings")]
    [SerializeField] private CropData cropData;
    public static Action<CropField> onFullySown;
    public static Action<CropField> onFullyWatered;
    public static Action<CropField> onFullyHarvested;
    private TileFieldState state;
    private int tilesSown;
    private int tilesWatered;
    private int tilesHarvested;
    private void Start()
    {
        scoreTiles();
    }
    private void scoreTiles()
    {
        for(int i=0;i<tilesParent.childCount;i++)
        {
            cropTiles.Add(tilesParent.GetChild(i).gameObject.GetComponent<CropTile>());
        }
    }
    public void seedCollidedCallback(Vector3[] seedPositions)
    {
        for(int i=0;i<seedPositions.Length;i++)
        {
            CropTile closestCropTile = getClosestCropTile(seedPositions[i]);
            if (closestCropTile == null)
                continue;
            if (!closestCropTile.isEmpty())
                continue;
            Sow(closestCropTile);
        }
    }
    public void waterCollidedCallback(Vector3[] waterPosition)
    {
        for (int i = 0; i < waterPosition.Length; i++)
        {
            CropTile closestCropTile = getClosestCropTile(waterPosition[i]);
            if (closestCropTile == null)
                continue;
            if (!closestCropTile.isSown())
                continue;
            Water(closestCropTile);
        }
    }
    private void Sow(CropTile closestCropTile)
    {
        closestCropTile.Sow(cropData);
        tilesSown ++;
        if (tilesSown == cropTiles.Count)
            fieldFullySown();
    }

    public void Harvest(Transform harvestSphere)
    {
        float harvestRadius = harvestSphere.localScale.x;
        for(int i=0;i< cropTiles.Count;i++)
        {
            if(cropTiles[i].isEmpty())
                continue ;
            float distance = Vector3.Distance(harvestSphere.position,cropTiles[i].transform.position);
            if (distance < harvestRadius)
                harvestTile(cropTiles[i]);
        }
    }

    private void harvestTile(CropTile cropTile)
    {
        cropTile.Harvest();
        tilesHarvested++;
        if (tilesHarvested == cropTiles.Count)
            fieldFullyHarvested();
    }

    private void Water(CropTile closestCropTile)
    {
        closestCropTile.Water();
        tilesWatered++;
        if (tilesWatered == cropTiles.Count)
            fieldFullyWatered();
    }
    private void fieldFullySown()
    {
        state = TileFieldState.Sown;
        onFullySown?.Invoke(this);
    }
    private void fieldFullyWatered()
    {
        state = TileFieldState.Watered;
        onFullyWatered?.Invoke(this);
    }
    private void fieldFullyHarvested()
    {
        state = TileFieldState.Empty;
        tilesSown = 0;
        tilesWatered = 0;
        tilesHarvested = 0;
        onFullyHarvested?.Invoke(this);
    }
    [NaughtyAttributes.Button]
    private void InstantlySowTiles()
    {
        for(int i=0;i<cropTiles.Count;i++)
            Sow(cropTiles[i]);
    }
    [NaughtyAttributes.Button]
    private void InstantlyWaterTiles()
    {
        for (int i = 0; i < cropTiles.Count; i++)
            Water(cropTiles[i]);
    }
    private CropTile getClosestCropTile(Vector3 seedPosition)
    {
        float minDistance = Mathf.Infinity;
        int cropTileClosestIndex = -1;
        for(int i=0;i<cropTiles.Count;i++)
        {
            float distanceToCropTile = Vector3.Distance(cropTiles[i].transform.position,seedPosition);
            if(distanceToCropTile < minDistance)
            {
                minDistance = distanceToCropTile;
                cropTileClosestIndex = i;
            }
        }

        if (cropTileClosestIndex == -1)
            return null;
        return cropTiles[cropTileClosestIndex];
    }
    public bool isEmpty()
    {
        return state == TileFieldState.Empty;
    }
    public bool isSown() => state == TileFieldState.Sown;
    public bool isWatered() => state == TileFieldState.Watered;
}
