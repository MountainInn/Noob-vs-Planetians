using UnityEngine;
using UnityEngine.Events;
using HyperCasual.Runner;
using System;

public class InteractiveCollider : Spawnable
{
    [SerializeField] UnityEvent<PlayerCharacter> onPlayerEnter;
    [SerializeField] OnEnemyEnter onEnemyEnter;
    [SerializeField] OnBulletEnter onBulletEnter;
    [SerializeField] UnityEvent onOtherEnter;

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
}
