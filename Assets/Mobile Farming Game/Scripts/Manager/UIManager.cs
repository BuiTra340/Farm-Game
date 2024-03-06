using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject treeModePanel;

    [SerializeField] private GameObject appleTree;
    [SerializeField] private GameObject toolBoxContainer;

    private void Start()
    {
        PlayerDetection.onEnterTreeZone += enterTreeZoneCallback;
        PlayerDetection.onExitTreeZone += exitTreeZoneCallback;
        AppleTreeManager.onTreeModeStarted += setTreeMode;
        AppleTreeManager.onTreeModeEnded += setGameMode;
        setGameMode();
    }
    private void OnDestroy()
    {
        PlayerDetection.onEnterTreeZone -= enterTreeZoneCallback;
        PlayerDetection.onExitTreeZone -= exitTreeZoneCallback;
        AppleTreeManager.onTreeModeStarted -= setTreeMode;
        AppleTreeManager.onTreeModeEnded -= setGameMode;
    }
    private void enterTreeZoneCallback(AppleTree tree)
    {
        appleTree.gameObject.SetActive(true);
        toolBoxContainer.gameObject.SetActive(false);
    }
    private void exitTreeZoneCallback(AppleTree tree)
    {
        toolBoxContainer.gameObject.SetActive(true);
        appleTree.gameObject.SetActive(false);
    }
    private void setGameMode()
    {
        gamePanel.SetActive(true);
        treeModePanel.SetActive(false);
    }
    private void setTreeMode(AppleTree tree)
    {
        gamePanel.SetActive(false);
        treeModePanel.SetActive(true);
    }
}
