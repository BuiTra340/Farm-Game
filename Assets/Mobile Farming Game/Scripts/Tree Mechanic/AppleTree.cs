using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject treeVCam;
    [SerializeField] private Renderer renderer;
    [SerializeField] private Transform appleParent;

    [Header("Settings")]
    [SerializeField] private float maxShakeMagnitude;
    [SerializeField] private float shakeIncrement;
    private float shakeSliderValue;
    private float shakeMagnitude;
    private AppleTreeManager treeManager;

    public void enableTreeCam() => treeVCam.SetActive(true);
    public void disableTreeCam() => treeVCam.SetActive(false);

    public void stopShake() => tweenShake(0);
    public void Shake()
    {
        tweenShake(maxShakeMagnitude);
        updateShakeSlider();
    }

    private void updateShakeSlider()
    {
        shakeSliderValue += shakeIncrement;
        treeManager.updateShakeSlider(shakeSliderValue);

        for(int i=0;i< appleParent.childCount;i++)
        {
            Apple apple = appleParent.GetChild(i).GetComponent<Apple>();
            if (apple.isFree())
                continue;
            float value = (float)i / appleParent.childCount;
            if(shakeSliderValue > value)
                apple.releaseApple();
        }
        if (shakeSliderValue >= 1)
        {
            exitTreeMode();
        }
    }
    public bool isReady()
    {
        bool ready = true;
        for (int i = 0; i < appleParent.childCount; i++)
            if (!appleParent.GetChild(i).GetComponent<Apple>().isReady())
                ready = false;
        return ready;
    }

    private void exitTreeMode()
    {
        treeManager.endedTreeMode();
        disableTreeCam();
        LeanTween.delayedCall(.1f, () => stopShake());
    }

    private void tweenShake(float maxShakeMagnitude)
    {
        LeanTween.cancel(renderer.gameObject);
        LeanTween.value(renderer.gameObject, updateShakeMagnitude, shakeMagnitude, maxShakeMagnitude, 1);
    }

    public void Initialize(AppleTreeManager treeManager)
    {
        this.treeManager = treeManager;
        shakeSliderValue = 0;
        enableTreeCam();
    }

    private void updateShakeMagnitude(float value)
    {
        shakeMagnitude = value;
        updateMaterials();
    }

    private void updateMaterials()
    {
        foreach(var mat in renderer.materials)
            mat.SetFloat("_Magnitude", shakeMagnitude);

        foreach (Transform appleT in appleParent)
        {
            Apple apple = appleT.GetComponent<Apple>();
            apple.updateMaterials(shakeMagnitude);
        }
    }
}
