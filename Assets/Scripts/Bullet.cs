using UnityEngine;
using DG.Tweening;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Harm))]
public class Bullet : MonoBehaviour, Harm.IOnHarmCallback
{
    [Inject] Pool pool;
    [Inject] public Harm harm;

    Tween moveTween;

    public void Initialize(int gunDamage, int range, float bulletSpeed)
    {
        harm.damage =
            new (PlayerCharacter.instance.harm.damage.AsFloorInt() * gunDamage)
            ;

        moveTween =
            transform
            .DOMoveZ(transform.position.z + range, bulletSpeed)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(Despawn);
    }

    void OnTriggerEnter(Collider other)
    {
        var itargets = other.GetComponents<ITarget>();

        if (itargets != null)
        {
            foreach (var item in itargets)
                item.OnHit(this);
      
            ImpactPS.instance.Fire(transform.position);

            Despawn();
        }
    }

    public void OnHarm(Mortal mortal)
    {
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

