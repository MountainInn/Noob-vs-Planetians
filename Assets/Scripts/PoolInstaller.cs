using Zenject;
using UnityEngine;

public class PoolInstaller : MonoInstaller
{
    [SerializeField] Bullet prefabBullet;
    [SerializeField] FollowingTrail prefabFollowingTrail;
    [Space]
    [SerializeField] [Min(6)] int initialSize = 6;

    override public void InstallBindings()
    {
        Container
            .BindMemoryPool<Bullet, Bullet.Pool>()
            .WithInitialSize(initialSize)
            .FromComponentInNewPrefab(prefabBullet)
            .UnderTransform(parent: null)
            .AsCached()
            .NonLazy()
            ;

        Container
            .BindMemoryPool<FollowingTrail, FollowingTrail.Pool>()
            .WithInitialSize(initialSize)
            .FromComponentInNewPrefab(prefabFollowingTrail)
            .UnderTransform(parent: null)
            .AsCached()
            .NonLazy()
            ;
    }
}
