using UnityEngine;
using Zenject;

public class PoolUser : MonoBehaviour
{
    static public PoolUser instance => _inst;
    static PoolUser _inst;
    PoolUser(){ _inst = this; }

    [Inject] public Bullet.Pool bulletPool;
    [Inject] public FollowingTrail.Pool trailPool;

}
