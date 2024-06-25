using UnityEngine;
using DG.Tweening;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Gameplay;



[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Harm))]
public class Bullet : MonoBehaviour, Harm.IOnHarmCallback
{
    // Pool pool;

    Tween moveTween;

    public Harm harm;

    public void Initialize(int damage, int range, float bulletSpeed)
    {
        harm = GetComponent<Harm>();

        harm.damage = damage;

        moveTween =
            transform
            .DOMoveZ(transform.position.z + range, bulletSpeed)
            .SetSpeedBased(true)
            .OnKill(Despawn);
    }

    void OnTriggerEnter(Collider other)
    {
        var hitboxes = other.GetComponents<ITarget>();

        if (hitboxes != null)
        {
            foreach (var item in hitboxes)
                item.OnHit(this);
        }
    }

    public void OnHarm(Mortal mortal)
    {
        Despawn();
    }

    void Despawn()
    {
        // pool.Despawn(this);
        GameObject.Destroy(gameObject);
    }
}
