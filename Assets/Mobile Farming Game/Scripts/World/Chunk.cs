using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject chunkUnlocked;
    [SerializeField] private GameObject chunkLocked;
    [SerializeField] private TextMeshPro priceText;
    private ChunkWall chunkWall;

    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool unlocked;
    [Header("Actions")]
    public static Action onChunkUnlocked;
    public static Action onPriceChange;

    [Header("Mesh Renderer")]
    [SerializeField] private MeshFilter chunkFilter;
    private void Awake()
    {
        chunkWall = GetComponent<ChunkWall>();
    }
    private void Start()
    {
        currentPrice = initialPrice;
        priceText.text = currentPrice.ToString();
    }

    public void tryUnlock()
    {
        if (CashManager.instance.getCurrentCoins() <= 0)
            return;
       currentPrice--;
        CashManager.instance.addCoin(-1);
        onPriceChange?.Invoke();
        priceText.text = currentPrice.ToString();
        if (currentPrice <= 0)
            unLock();
    }
    public void Initialze(int _currentPrice)
    {
        currentPrice = _currentPrice;
        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            unLock(false);
    }
    private void unLock(bool triggerAction = true)
    {
        chunkUnlocked.SetActive(true);
        chunkLocked.SetActive(false);
        unlocked = true;

        if(triggerAction)
            onChunkUnlocked?.Invoke();
    }
    public void updateWalls(int configuration) => chunkWall.updateWall(configuration);
    public bool isUnlocked() => unlocked;
    public int getInitialPrice() => initialPrice;
    public int getCurrentPrice() => currentPrice;

    public void displayLockedElements()
    {
        chunkLocked.SetActive(true);
    }

    public void setRenderer(Mesh chunkMesh)
    {
        chunkFilter.mesh = chunkMesh;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);
        Gizmos.color = new Color(0, 0, 0,0);
        Gizmos.DrawCube(transform.position, Vector3.one * 5);
    }
}
