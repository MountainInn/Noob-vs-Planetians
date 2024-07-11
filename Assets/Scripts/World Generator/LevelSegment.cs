using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] [Min(1)] float length;
    [Space]
    [SerializeField] List<Chunk> chunks;

    [HideInInspector] [SerializeField] public List<Chunk> spawnedChunks = new();
    [HideInInspector] public float currentLength = 0;

    public void ReparentChunks(Transform parent)
    {
        foreach (var item in spawnedChunks)
        {
            if (item == null)
                continue;

            item.transform.SetParent(parent);
        }
    }

    public float Generate(float zPosition)
    {
        ClearChunks();

        currentLength = 0;

        var rolledChunks =
            chunks
            .Shuffle()
            .TakeWhile(ch => (currentLength += ch.length) <= length);

        foreach (var item in rolledChunks)
        {
            var newChunk = Chunk.InstantiateChunk(zPosition + item.length / 2, item);

            spawnedChunks.Add(newChunk);

            zPosition += item.length;
        }

        return zPosition;
    }

    public void ClearChunks()
    {
        spawnedChunks.DestroyAll();
    }
}
