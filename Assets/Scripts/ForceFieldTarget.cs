using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceFieldTarget : MonoBehaviour
{
    [SerializeField] ForceField forceField;

    Rigidbody rb;
    bool toggle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        forceField = ForceField.instance;
    }

    public void Toggle(bool toggle)
    {
        rb.isKinematic = !toggle;
        this.toggle = toggle;
    }

    public void SetForceField(ForceField forceField)
    {
        this.forceField = forceField;
    }

    void FixedUpdate()
    {
        if (!toggle)
            return;

        Vector3 force =
            forceField.AttractionForce(transform)
            * Time.fixedDeltaTime;

        rb.AddForce(force, ForceMode.Impulse);
    }
}
