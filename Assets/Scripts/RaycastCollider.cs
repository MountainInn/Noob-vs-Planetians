using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class RaycastCollider : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [Space]
    [SerializeField] float raycastLength = 20f;
    [SerializeField] float radius;
    [SerializeField] Vector3 offset;
    [Space]
    [SerializeField] bool startRaycasting;
    [SerializeField] int raycastBufferSize = 6;
    [Space]
    [SerializeField] UnityEvent<Collider> onCollision;
    [Space]
    [SerializeField] CapsuleCollider col;
    [SerializeField] Rigidbody rb;

    Vector3 previousPosition;
    Vector3 startPosition => transform.position + offset;

    RaycastHit[] hits;
    RaycastHit closestValidHit;

    bool raycasting;
    bool hasValidHits;

    void Awake()
    {
        hits = new RaycastHit[raycastBufferSize];
    }

    public void Reinitialize(Vector3 initialPosition)
    {
        previousPosition = initialPosition + offset;

        raycasting = true;
    }

    void OnDisable()
    {
        raycasting = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       
        Vector3 to = previousPosition + transform.forward * raycastLength;
       
        Gizmos.DrawLine(previousPosition, to);

        Gizmos.DrawSphere(to, radius);
        
        Gizmos.color = Color.green;

        Gizmos.DrawRay(previousPosition, Vector3.up);
    }

    void FixedUpdate()
    {
        if (hasValidHits)
        {
            Vector3 directionToHit = (closestValidHit.point - transform.position).normalized;

            float dot = Vector3.Dot(transform.forward, directionToHit);

            if (dot < 0)
            {
                Collide();
            }
        }

        Raycast();

        previousPosition = transform.position;
    }

    void Raycast()
    {
        hasValidHits = false;

        int hitCount = 0;

        if (raycasting)
        {
            hitCount = 
                Physics
                .SphereCastNonAlloc(previousPosition,
                                    radius,
                                    transform.forward,
                                    hits,
                                    raycastLength,
                                    layerMask,
                                    QueryTriggerInteraction.Collide);

            hasValidHits = (hitCount > 0);
        }

        if (hasValidHits)
        {
            try
            {
                var (index, hit) =
                    hits
                    .Take(hitCount)
                    .Enumerate()
                    .OrderBy(tuple => tuple.value.distance)
                    .First(tuple => tuple.value.point != Vector3.zero);

                closestValidHit = hit;
            }
            catch (System.Exception e)
            {
                if (e is System.InvalidOperationException)
                    hasValidHits = false;
                else
                    throw e;
            }

            if (closestValidHit.point == Vector3.zero)
            {
                hasValidHits = false;
            }
        }
    }

    void Collide()
    {
        ICollidable collidable =
            (ICollidable)closestValidHit.collider?.GetComponent(typeof(ICollidable))
            ??
            (ICollidable)closestValidHit.rigidbody?.GetComponent(typeof(ICollidable))
            ;

        
        bool isEqualsNull = (collidable?.Equals(null) ?? true);

        if (isEqualsNull)
            return;
        
        collidable.OnRaycastCollide(this);

        onCollision?.Invoke(closestValidHit.collider);
    }

    public interface ICollidable
    {
        void OnRaycastCollide(RaycastCollider raycastCollider);
    }
}
