using UnityEngine;
using DG.Tweening;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using Zenject;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Damage))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [Space]
    [SerializeField] public float force;
    [Space]
    [SerializeField] public Damage damage;

    [Inject] Pool pool;
    [Inject] FollowingTrail.Pool trailPool;

    Tween moveTween;

    TrailRenderer trail;

    void Awake()
    {
        damage = GetComponent<Damage>();
    }

    public void Initialize(int gunDamage, int range, float bulletSpeed)
    {
        damage.Value.SetInitial(gunDamage);

        trail =
            PoolUser.instance.trailPool
            .Spawn(transform)
            .GetComponent<TrailRenderer>();

        moveTween =
            transform
            .DOMoveZ(transform.position.z + range, bulletSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(Despawn);
    }

    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke();
       
        ImpactPS.instance.Fire(transform.position, 6);

        Despawn();
    }

    void Despawn()
    {
        moveTween?.Kill();

        trail.transform.SetParent(null, true);

        pool.Despawn(this);
    }

    public class Pool : MonoMemoryPool<Gun, Transform, Bullet>
    {
        protected override void Reinitialize(Gun gun, Transform muzzle, Bullet item)
        {
            item.transform.position = muzzle.position;
            item.transform.forward = Vector3.forward;

            item.Initialize(gun.totalDamage, gun.totalRange, gun.bulletSpeed);
        }
    }
}

