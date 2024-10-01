using UnityEngine;
using UnityEngine.Events;
using HyperCasual.Runner;
using System;
using System.Collections.Generic;

public class InteractiveCollider : Spawnable, RaycastCollider.ICollidable
{
    [SerializeField] public UnityEvent<PlayerCharacter> onPlayerEnter;
    [SerializeField] public OnEnemyEnter onEnemyEnter;
    [SerializeField] public OnBulletEnter onBulletEnter;
    [SerializeField] public UnityEvent onOtherEnter;

    [SerializeField] public List<Field> CurrencyEvents;
    
    [Serializable]
    public class Field
    {
        [SerializeField] public Currency currency;
        [SerializeField] public UnityEvent onAdd;
        [SerializeField] public UnityEvent onSubtract;

    }

    [Serializable] public class OnBulletEnter : UnityEvent<Bullet> {  }
    [Serializable] public class OnEnemyEnter : UnityEvent<Enemy> {  }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter player))
        {
            onPlayerEnter?.Invoke(player);
        }
        else if (other.TryGetComponent(out Enemy enemy))
        {
            onEnemyEnter?.Invoke(enemy);
        }
        else if (other.TryGetComponent(out Bullet bullet))
        {
            onBulletEnter?.Invoke(bullet);
        }
        else
        {
            onOtherEnter?.Invoke();
        }
    }

    public void OnRaycastCollide(RaycastCollider raycastCollider)
    {
        if (raycastCollider.TryGetComponent(out Bullet bullet))
        {
            onBulletEnter?.Invoke(bullet);
        }
    }
}
