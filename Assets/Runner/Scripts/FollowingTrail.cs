using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class FollowingTrail : MonoBehaviour
{
    TrailRenderer trail;

    public Transform target;

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
                Destroy(gameObject);

            return;
        }

        transform.position = target.position;
    }
}
