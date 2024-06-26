using UnityEngine;
using Zenject;

public class PoolUser : MonoBehaviour
{
    static public PoolUser instance => _inst ??= FindObjectOfType<PoolUser>();
    static PoolUser _inst;

    [Inject] public Bullet.Pool bulletPool;
    [Inject] public FollowingTrail.Pool trailPool;

}
