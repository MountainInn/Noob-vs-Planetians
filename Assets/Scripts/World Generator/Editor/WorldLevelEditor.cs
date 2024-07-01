using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(WorldLevel))]
public class WorldLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WorldLevel wl = (WorldLevel)target;

        if (GUILayout.Button("Generate"))
        {
            wl.Generate();
        }

        if (GUILayout.Button("Parent Chunks"))
        {
            wl.ParentChunks();
        }
    }
}
