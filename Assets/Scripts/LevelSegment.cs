using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] [Min(1)] float length;
    [Space]
    [SerializeField] List<Chunk> chunks;

    public float currentLength = 0;

    public float Generate(float zPosition)
    {
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
                                       transform);

            position.z += item.length / 2f;
        }

        return position.z;
    }
}
