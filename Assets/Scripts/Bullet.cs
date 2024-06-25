using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Bullet : MonoBehaviour
{
    // Pool pool;

    [HideInInspector] public int damage;

    Tween moveTween;

    public void Initialize(int damage, int range, float bulletSpeed)
    {
        this.damage = damage;

        moveTween =
            transform
            .DOMoveZ(transform.position.z + range, bulletSpeed)
            .SetSpeedBased(true)
            .OnKill(Despawn);
    }

    void OnTriggerEnter(Collider other)
    {
        IHitbox[] hitboxes = other.GetComponents<IHitbox>();

        if (hitboxes != null && hitboxes.Length > 0)
        {
            hitboxes.Map(hitbox => hitbox.OnHit(this));

            Despawn();
        }
    }

    void Despawn()
    {
        // pool.Despawn(this);
        GameObject.Destroy(gameObject);
    }
}
