using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Chunk : MonoBehaviour
{
    [SerializeField] public float width;
    [SerializeField] public float length;
    [SerializeField] public float height;
    [Space]
    [SerializeField] Transform floor;

    public GameObject Floor => floor.gameObject;

    void OnValidate()
    {
        if (floor)
        {
            floor.transform.localScale = new Vector3(width, height, length);
        }
    }

    static public Chunk InstantiateChunk(float zPosition, Object prefabChunk)
    {
        Chunk newChunk;

#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            RuntimeInstantiate();
        }
        else
        {
            newChunk = (Chunk)PrefabUtility.InstantiatePrefab(prefabChunk, null);
            newChunk.transform.position = Vector3.zero.WithZ(zPosition);
            newChunk.transform.rotation = Quaternion.identity;
        }
#else
        RuntimeInstantiate();
#endif

        void RuntimeInstantiate()
        {
            newChunk = (Chunk)GameObject.Instantiate(prefabChunk,
                                                     Vector3.zero.WithZ(zPosition),
                                                     Quaternion.identity,
                                                     null);
        }

        return newChunk;
    }

}
