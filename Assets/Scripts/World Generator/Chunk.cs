using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] MeshRenderer floor;
    [SerializeField] Collider coll;

    [SerializeField] public float length;

    void OnValidate()
    {
        if (!(coll.bounds.size.z == 0))
            length = coll.bounds.size.z;
    }
}
