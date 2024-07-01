using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] [Min(1)] float length;
    [Space]
    [SerializeField] List<Chunk> chunks;

    [HideInInspector] [SerializeField] List<Chunk> spawnedChunks = new();
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

        Vector3 position = default;
        position.z = zPosition;

        foreach (var item in rolledChunks)
        {
            var newChunk = Instantiate(item,
                                       position,
                                       Quaternion.identity,
                                       null);

            spawnedChunks.Add(newChunk);

            position.z += item.length;
        }

        return position.z;
    }

    public void ClearChunks()
    {
        foreach (var item in spawnedChunks)
        {
            if (item == null)
                continue;

            if (Application.isPlaying)
                Destroy(item.gameObject);
            else
                DestroyImmediate(item.gameObject, true);
        }
        spawnedChunks.Clear();
    }
}
