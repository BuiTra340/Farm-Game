using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvestAbility : MonoBehaviour
{
    [Header("Elements")]
    private PlayerAnimator playerAnimator;
    [SerializeField] private Transform harvestSphere;
    [Header("Settings")]
    private CropField currentCropField;
    private PlayerToolSelector playerToolSelector;
    private bool canHarvest;
    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();
        CropField.onFullyHarvested += cropFieldFullyCallback;
        playerToolSelector.onToolSelected += toolSelectedCallback;
    }
    private void OnDestroy()
    {
        CropField.onFullyHarvested -= cropFieldFullyCallback;
        playerToolSelector.onToolSelected -= toolSelectedCallback;
    }
    private void cropFieldFullyCallback(CropField cropField)
    {
        if (currentCropField == cropField)
            playerAnimator.stopHarvestAnimation();
    }
    //private void WaterCollidedCallback(Vector3[] waterPosition)
    //{
    //    if (currentCropField == null)
    //        return;
    //    currentCropField.waterCollidedCallback(waterPosition);
    //}
    private void toolSelectedCallback(PlayerToolSelector.Tool toolSelector)
    {
        if (!playerToolSelector.canHarvest())
            playerAnimator.stopHarvestAnimation();
    }
    private void enteredCropField(CropField cropField)
    {
        if (playerToolSelector.canHarvest() && cropField.isWatered())
        {
            if (currentCropField == null)
                currentCropField = cropField;
            playerAnimator.playHarvestAnimation();

            if (canHarvest)
                currentCropField.Harvest(harvestSphere);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") && other.GetComponent<CropField>().isEmpty())
        {
            currentCropField = other.GetComponent<CropField>();
            enteredCropField(currentCropField);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            enteredCropField(other.GetComponent<CropField>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.stopHarvestAnimation();
            currentCropField = null;
        }
    }
    public void startHarvesting()
    {
        canHarvest = true;
    }
    public void stopHarvesting()
    {
        canHarvest = false;
    }
}
