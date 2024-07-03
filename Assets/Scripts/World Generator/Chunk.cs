using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Chunk : MonoBehaviour
{
    [SerializeField] MeshRenderer floor;
    [SerializeField] Collider coll;
    [Space]
    [SerializeField] public float width;
    [SerializeField] public float length;
    [SerializeField] public float height;

    public GameObject Floor => coll.gameObject;

    void OnValidate()
    {
        if (!(coll.bounds.size.x == 0))
            width = coll.bounds.size.x;

        if (!(coll.bounds.size.y == 0))
            height = coll.bounds.size.y;
       
        if (!(coll.bounds.size.z == 0))
            length = coll.bounds.size.z;
    }

    static public Chunk InstantiateChunk(float zPosition, Object prefabChunk)
    {
        Chunk newChunk;

#if UNITY_EDITOR
        newChunk = (Chunk)PrefabUtility.InstantiatePrefab(prefabChunk, null);
        newChunk.transform.position = Vector3.zero.WithZ(zPosition);
        newChunk.transform.rotation = Quaternion.identity;
#else
        newStartChunk = Instantiate(startChunk,
                                    Vector3.zero.WithZ(zPosition),
                                    Quaternion.identity,
                                    null);
#endif
        return newChunk;
    }

}
