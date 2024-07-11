using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] [Min(1)] float length;
    [Space]
    [SerializeField] List<Field> fields;

    [HideInInspector] [SerializeField] public List<Chunk> spawnedChunks = new();
    [HideInInspector] public float currentLength = 0;

    [System.Serializable]
    public class Field
    {
        [SerializeField] public Chunk chunk;
        [SerializeField] [Range(0, 1f)] public float probability = 1f;
        [SerializeField] public bool repeatable = false;
        [NonSerialized] public bool taken = false;

    }

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
            fields
            .Map(f => f.taken = false)
            .ToList()
            .InfiniteStream()
            .Where(f => f.repeatable || !f.taken)
            .Where(f => UnityEngine.Random.value < f.probability)
            .Select(f =>
            {
                currentLength += f.chunk.length;

                f.taken = true;

                return f.chunk;
            })
            .TakeWhile(_ => currentLength < length)
            .Shuffle()
            ;

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
