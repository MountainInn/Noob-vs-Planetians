using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorldLevel : MonoBehaviour
{
    [SerializeField] Chunk startChunk;
    [SerializeField] Chunk finishChunk;

    public LevelSegment[] Segments => GetComponents<LevelSegment>();

    public void Generate()
    {
        GetComponentsInChildren<Transform>(true)
            .Where(t => t != transform)
            .ToList()
            .DestroyAll();

        float zPosition = 0;

        Chunk newStartChunk = Chunk.InstantiateChunk(zPosition, startChunk);
        newStartChunk.transform.SetParent(transform);

        zPosition += newStartChunk.length / 2;

        foreach (var item in Segments)
        {
            item.ClearChunks();
            zPosition = item.Generate(zPosition);

            item.ReparentChunks(transform);
        }

        Chunk newFinishChunk = Chunk.InstantiateChunk(zPosition + finishChunk.length / 2,
                                                      finishChunk);
        newFinishChunk.transform.SetParent(transform);
    }
}
