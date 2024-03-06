using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterAbility : MonoBehaviour
{
    [Header("Elements")]
    private PlayerAnimator playerAnimator;
    [Header("Settings")]
    private CropField currentCropField;
    private PlayerToolSelector playerToolSelector;
    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();
        WaterParticles.onWaterCollided += WaterCollidedCallback;

        CropField.onFullyWatered += cropFieldFullyCallback;
        playerToolSelector.onToolSelected += toolSelectedCallback;
    }
    private void OnDestroy()
    {
        WaterParticles.onWaterCollided -= WaterCollidedCallback;

        CropField.onFullyWatered -= cropFieldFullyCallback;
        playerToolSelector.onToolSelected -= toolSelectedCallback;
    }
    private void cropFieldFullyCallback(CropField cropField)
    {
        if (currentCropField == cropField)
            playerAnimator.stopWaterAnimation();
    }
    private void WaterCollidedCallback(Vector3[] waterPosition)
    {
        if (currentCropField == null)
            return;
        currentCropField.waterCollidedCallback(waterPosition);
    }
    private void toolSelectedCallback(PlayerToolSelector.Tool toolSelector)
    {
        if (!playerToolSelector.canWater())
            playerAnimator.stopWaterAnimation();
    }
    private void enteredCropField(CropField cropField)
    {
        if (playerToolSelector.canWater() && cropField.isSown())
        {
            if(currentCropField == null)
                currentCropField = cropField;
            playerAnimator.playWaterAnimation();
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
            playerAnimator.stopWaterAnimation();
            currentCropField = null;
        }
    }
}
