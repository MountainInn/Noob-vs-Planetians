using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Damage))]
public class Repeler : MonoBehaviour
{
    [SerializeField] float force;
    [Space]
    [SerializeField] UnityEvent onRepel;

    Damage harm;

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        harm = GetComponent<Damage>();
    }

    public void __Repel(Rigidbody rb) => Repel(rb);
    public void Repel(Rigidbody rb)
    {
        var approximateCollisionPoint = col.ClosestPoint(rb.transform.position);

        Vector3 runnerPosition = rb.transform.position;

        var pushBackForce =
            (runnerPosition - approximateCollisionPoint).normalized
            * force;

        rb.AddForceAtPosition(pushBackForce,
                              approximateCollisionPoint,
                              ForceMode.Impulse);

        onRepel?.Invoke();
    }
}
