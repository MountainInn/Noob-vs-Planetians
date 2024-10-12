using UnityEngine;

public class ForceField : MonoBehaviour
{
    static public ForceField instance => _inst ??= FindObjectOfType<ForceField>();
    static ForceField _inst;

    [SerializeField] public float force;

    public Vector3 AttractionForce(Transform targetTransform)
    {
        return
            (transform.position - targetTransform.position).normalized
            * force;
    }
}
