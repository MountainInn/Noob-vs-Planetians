using UnityEngine;
using DG.Tweening;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using Zenject;
using UnityEngine.Events;


[RequireComponent(typeof(Damage))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [Space]
    [Inject] Pool pool;
    [Inject] public Damage damage;

    Tween moveTween;

    public void Initialize(int gunDamage, int range, float bulletSpeed)
    {
        damage.Value.SetInitial(gunDamage);

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
       
        ImpactPS.instance.Fire(transform.position);

        Despawn();
    }

    void Despawn()
    {
        moveTween?.Kill();
       
        pool.Despawn(this);
    }

    public class Pool : MonoMemoryPool<Gun, Transform, Bullet>
    {
        protected override void Reinitialize(Gun gun, Transform muzzle, Bullet item)
        {
            item.transform.position = muzzle.position;
            item.transform.forward = Vector3.forward;

            base.Reinitialize(gun, muzzle, item);

            item.Initialize(gun.totalDamage, gun.totalRange, gun.bulletSpeed);
        }
    }
}

