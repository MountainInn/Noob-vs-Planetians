using UnityEngine;
using System.Linq;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Rigidbody root;

    Rigidbody[] boneRbs;
    Collider[] boneColliders;

    Rigidbody parentRb;
    Collider parentCollider;

    bool isToggled = false;
    Vector3 storedForce;

    void Awake()
    {
        parentRb = GetComponent<Rigidbody>();
        parentCollider = GetComponent<Collider>();

        boneRbs =
            GetComponentsInChildren<Rigidbody>()
            .ToArray();

        boneColliders =
            boneRbs
            .Where(rb => rb != parentRb)
            .Select(rb => rb.GetComponent<Collider>())
            .ToArray();
    }

    public void Activate(bool toggle)
    {
        isToggled = toggle;

        foreach (var rb in boneRbs)
        {
            rb.isKinematic = !toggle;
        }

        foreach (var col in boneColliders)
        {
            col.isTrigger = !toggle;
        }

        parentCollider.enabled = !toggle;
    }

    public void __StoreBulletForce(Bullet Bullet) => StoreBulletForce(Bullet);
    public void StoreBulletForce(Bullet bullet)
    {
        storedForce =
            (transform.position - bullet.transform.position)
            .AddX(.2f)
            .WithY(0)
            .normalized * bullet.force;

        if (isToggled)
        {
            root.AddForce(storedForce, ForceMode.Impulse);
        }
    }
}
