using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWall : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;
    public void updateWall(int configuration)
    {
        frontWall.SetActive(isKthBitSet(configuration, 0));
        rightWall.SetActive(isKthBitSet(configuration, 1));
        backWall.SetActive(isKthBitSet(configuration, 2));
        leftWall.SetActive(isKthBitSet(configuration, 3));
    }
    private bool isKthBitSet(int configuation,int k)
    {
        if((configuation & (1 << k)) > 0)
            return false;
        return true;
    }
}
