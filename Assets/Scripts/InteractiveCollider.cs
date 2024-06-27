using UnityEngine;
using UnityEngine.Events;
using HyperCasual.Runner;

public class InteractiveCollider : Spawnable
{
    [SerializeField] UnityEvent onPlayerEnter;
    [SerializeField] UnityEvent<Enemy> onEnemyEnter;
    [SerializeField] UnityEvent<Bullet> onBulletEnter;
    [SerializeField] UnityEvent onOtherEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter player))
        {
            onPlayerEnter?.Invoke();
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
