using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(Chunk))]  
public class ChunkEditor : Editor
{
    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        Chunk chunk = (Chunk)target;
        Handles.Label(chunk.transform.position, target.name,style);
    }
}
#endif
