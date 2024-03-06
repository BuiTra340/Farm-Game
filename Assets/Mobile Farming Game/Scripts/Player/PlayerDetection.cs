using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDetection : MonoBehaviour
{
    [Header("Actions")]
    public static Action<AppleTree> onEnterTreeZone;
    public static Action<AppleTree> onExitTreeZone;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChunkTrigger"))
        {
            Chunk chunk = other.GetComponentInParent<Chunk>();
            chunk.tryUnlock();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out AppleTree tree))
        {
            onEnterTreeZone?.Invoke(tree);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AppleTree tree))
        {
            onExitTreeZone?.Invoke(tree);
            tree.disableTreeCam();
        }
    }
}
