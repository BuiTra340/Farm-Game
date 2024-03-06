using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSowAbility : MonoBehaviour
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
        SeedParticles.onSeedCollided += seedCollidedCallback;
        CropField.onFullySown += cropFieldFullyCallback;
        playerToolSelector.onToolSelected += toolSelectedCallback;
    }
    private void OnDestroy()
    {
        SeedParticles.onSeedCollided -= seedCollidedCallback;
        CropField.onFullySown -= cropFieldFullyCallback;
        playerToolSelector.onToolSelected -= toolSelectedCallback;
    }
    private void cropFieldFullyCallback(CropField cropField)
    {
        if(currentCropField == cropField)
            playerAnimator.stopSowAnimation();
    }
    private void seedCollidedCallback(Vector3[] seedPosition)
    {
        if (currentCropField == null)
            return;
        currentCropField.seedCollidedCallback(seedPosition);
    }
    private void toolSelectedCallback(PlayerToolSelector.Tool toolSelector)
    {
        if(!playerToolSelector.canSow())
            playerAnimator.stopSowAnimation();
    }
    private void enteredCropField(CropField cropField)
    {
        if (playerToolSelector.canSow() && cropField.isEmpty())
            playerAnimator.playSowAnimation();
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
            playerAnimator.stopSowAnimation();
            currentCropField = null;
        }
    }
}
