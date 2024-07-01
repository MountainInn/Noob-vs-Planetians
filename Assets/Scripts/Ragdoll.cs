using UnityEngine;
using System.Linq;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] bones;

    void Awake()
    {
        Rigidbody parentRb = GetComponent<Rigidbody>();

        bones =
            GetComponentsInChildren<Rigidbody>()
            .Where(rb => rb != parentRb)
            .ToArray();
    }

    public void Activate(bool toggle)
    {
        foreach (var item in bones)
        {
            item.isKinematic = toggle;
        }
    }
}
