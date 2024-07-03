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

        Chunk newStartChunk = Chunk.InstantiateChunk(zPosition, startChunk);

        zPosition += newStartChunk.length / 2;

        foreach (var item in Segments)
        {
            item.ClearChunks();
            zPosition = item.Generate(zPosition);
        }

        Chunk newFinishChunk = Chunk.InstantiateChunk(zPosition + finishChunk.length / 2,
                                                      finishChunk);

        spawnedChunks.Add(newStartChunk);
        spawnedChunks.Add(newFinishChunk);
    }

}
