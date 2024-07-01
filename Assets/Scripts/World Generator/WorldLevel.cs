using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldLevel : MonoBehaviour
{
    [SerializeField] Chunk startChunk;
    [SerializeField] Chunk finishChunk;

    [SerializeField] public List<Chunk> spawnedChunks = new();

    public LevelSegment[] Segments => GetComponents<LevelSegment>();

    public void ParentChunks()
    {
        foreach (var item in Segments)
        {
            item.ReparentChunks(transform);
        }

        foreach (var item in spawnedChunks)
        {
            item.transform.SetParent(transform);
        }
    }

    public void Generate()
    {
        spawnedChunks?.DestroyAll();

        float zPosition = 0;

        var newStartChunk = Instantiate(startChunk,
                                        Vector3.zero.WithZ(zPosition),
                                        Quaternion.identity,
                                        null);

        zPosition += newStartChunk.length / 2;

        foreach (var item in Segments)
        {
            item.ClearChunks();
            zPosition = item.Generate(zPosition);
        }

        var newFinishChunk = Instantiate(finishChunk,
                                         Vector3.zero.WithZ(zPosition + finishChunk.length / 2),
                                         Quaternion.identity,
                                         null);

        spawnedChunks.Add(newStartChunk);
        spawnedChunks.Add(newFinishChunk);
    }
}
