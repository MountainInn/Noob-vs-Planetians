using UnityEngine;
using DG.Tweening;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using Zenject;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [Space]
    [Inject] Pool pool;

    Tween moveTween;

    public void Initialize(int gunDamage, int range, float bulletSpeed)
    {
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

    public class Pool : MonoMemoryPool<GunSO, float, Transform, Bullet>
    {
        protected override void Reinitialize(GunSO gunSO, float speed, Transform muzzle, Bullet item)
        {
            item.transform.position = muzzle.position;
            item.transform.forward = Vector3.forward;

            base.Reinitialize(gunSO, speed, muzzle, item);

            item.Initialize(gunSO.damage, gunSO.range, speed);
        }
    }
}

