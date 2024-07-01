using UnityEngine;
using System.Collections.Generic;

public class WorldLevel : MonoBehaviour
{
    LevelSegment[] segments;

    void Awake()
    {
        segments = GetComponents<LevelSegment>();
    }

    public void ParentChunks()
    {
        foreach (var item in GetComponents<LevelSegment>())
        {
            item.ReparentChunks(transform);
        }
    }

    public void Generate()
    {
        segments = GetComponents<LevelSegment>();

        float zPosition = 0;

        foreach (var item in segments)
        {
            item.ClearChunks();
            zPosition = item.Generate(zPosition);
        }
    }
}
