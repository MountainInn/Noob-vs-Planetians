using UnityEngine;
using Zenject;

[RequireComponent(typeof(TrailRenderer))]
public class FollowingTrail : MonoBehaviour
{
    TrailRenderer trail;

    [Inject] Pool pool;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        trail.emitting = true;
    }

    void FixedUpdate()
    {
        if (trail.positionCount == 0)
            pool.Despawn(this);
    }

    public class Pool : MonoMemoryPool<Transform, FollowingTrail>
    {
        protected override void Reinitialize(Transform muzzle, FollowingTrail item)
        {
            item.transform.SetParent(muzzle);
            item.transform.localPosition = Vector3.zero;
        }
    }
}
