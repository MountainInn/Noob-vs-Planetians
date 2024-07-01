using UnityEngine;
using System.Collections.Generic;

public class WorldLevel : MonoBehaviour
{
    [SerializeField] List<LevelSegment> segments;

    [ContextMenu("Generate")]
    public void Generate()
    {
        float zPosition = 0;

        foreach (var item in segments)
        {
            zPosition = item.Generate(zPosition);
        }
    }
}
