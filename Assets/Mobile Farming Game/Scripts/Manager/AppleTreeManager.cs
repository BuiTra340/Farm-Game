using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider shakeSlider; 
    [Header("Settings")]
    private AppleTree lastTriggeredTree;

    [Header("Actions")]
    public static Action<AppleTree> onTreeModeStarted;
    public static Action onTreeModeEnded;
    private void Start()
    {
        PlayerDetection.onEnterTreeZone += enterTreeZoneCallback;
    }

    private void OnDestroy()
    {
        PlayerDetection.onEnterTreeZone -= enterTreeZoneCallback;
    }

    private void enterTreeZoneCallback(AppleTree tree)
    {
        lastTriggeredTree = tree;
    }

    // reference to button 
    public void startTreeMode()
    {
        if (!lastTriggeredTree.isReady())
            return;
        lastTriggeredTree.Initialize(this);
        shakeSlider.value = 0;
        onTreeModeStarted?.Invoke(lastTriggeredTree);
        updateShakeSlider(0);
    }
    public void updateShakeSlider(float value) => shakeSlider.value = value;

    public void endedTreeMode()
    {
        //lastTriggeredTree = null;
        onTreeModeEnded?.Invoke();
    }
}
