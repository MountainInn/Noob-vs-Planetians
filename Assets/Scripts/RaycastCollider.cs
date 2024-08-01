using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class RaycastCollider : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [Space]
    [SerializeField] Vector3 capsulePointOne;
    [SerializeField] Vector3 capsulePointTwo;
    [SerializeField] float radius;
    [SerializeField] Vector3 offset;
    [Space]
    [SerializeField] bool startRaycasting;
    [Space]
    [SerializeField] UnityEvent<Collider> onCollision;
    [Space]
    [SerializeField] CapsuleCollider col;
    [SerializeField] Rigidbody rb;

    Vector3 previousPosition;
    Vector3 startPosition => transform.position + offset;

    RaycastHit[] hits = new RaycastHit[3];
    RaycastHit closestValidHit;

    bool raycasting;
    bool hasValidHits;

    void OnEnable()
    {
        previousPosition = startPosition;

        raycasting = true;
    }

    void OnDisable()
    {
        raycasting = false;

        hasValidHits = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       
        Vector3 to = startPosition + transform.forward * 10f;
       
        Gizmos.DrawLine(startPosition, to);

        Gizmos.DrawSphere(to, radius);
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

        hasValidHits = false;

        if (raycasting)
        {
            var infos =
                Physics
                .SphereCastAll(previousPosition,
                               radius,
                               transform.forward,
                               10f,
                               layerMask,
                               QueryTriggerInteraction.Collide);

            hasValidHits = infos != null;

            hits = infos;
        }

        if (hasValidHits)
        {
            try
            {
                var (index, hit) =
                    hits
                    .Enumerate()
                    .OrderBy(tuple => tuple.value.distance)
                    .First();

                this.closestValidHit = hit;
            }
            catch (System.Exception e)
            {
                if (e is System.InvalidOperationException)
                    hasValidHits = false;
                else
                    throw e;
            }

            if (closestValidHit.point == Vector3.zero)
                hasValidHits = false;
        }

        previousPosition = transform.position;
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
