using Zenject;
using UnityEngine;

public class PoolInstaller : MonoInstaller
{
    [SerializeField] Bullet prefabBullet;
    [SerializeField] FollowingTrail prefabFollowingTrail;

    override public void InstallBindings()
    {
        Container
            .BindMemoryPool<Bullet, Bullet.Pool>()
            .FromComponentInNewPrefab(prefabBullet)
            .UnderTransform(parent: null)
            .AsCached()
            ;

        Container
            .BindMemoryPool<FollowingTrail, FollowingTrail.Pool>()
            .FromComponentInNewPrefab(prefabFollowingTrail)
            .UnderTransform(parent: null)
            .AsCached()
            ;
    }
}
