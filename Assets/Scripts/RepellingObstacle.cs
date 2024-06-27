using UnityEngine;

public class RepellingObstacle : MonoBehaviour
{
    [SerializeField] float force;

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            var approximateCollisionPoint = col.ClosestPoint(playerCharacter.transform.position);

            Vector3 runnerPosition = playerCharacter.transform.position;

            var pushBackForce =
                (runnerPosition - approximateCollisionPoint).normalized
                * force;

            playerCharacter
                .GetComponent<Rigidbody>()
                .AddForceAtPosition(pushBackForce, approximateCollisionPoint, ForceMode.Impulse);
        }
    }
}
