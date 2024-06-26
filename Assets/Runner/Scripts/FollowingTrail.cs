using UnityEngine;
using Zenject;

[RequireComponent(typeof(TrailRenderer))]
public class FollowingTrail : MonoBehaviour
{
    TrailRenderer trail;

    public Transform target;

    [Inject] Pool pool;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        trail.emitting = true;

        SafeUpdatePosition();
    }

    void FixedUpdate()
    {
        SafeUpdatePosition();
    }

    void SafeUpdatePosition()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            if (trail.positionCount == 0)
                // pool.Despawn(this);
                GameObject.Destroy(gameObject);

            return;
        }

        transform.position = target.position;
    }

    public class Pool : MonoMemoryPool<Transform, Transform, FollowingTrail>
    {
        protected override void Reinitialize(Transform target, Transform muzzle, FollowingTrail item)
        {
            item.transform.position = muzzle.position;

            item.target = target;
        }
    }
}
