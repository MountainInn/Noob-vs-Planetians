using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] MeshRenderer floor;
    [SerializeField] Collider coll;

    public float length => coll.bounds.size.z;
}
