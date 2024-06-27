using UnityEngine;
using DG.Tweening;
using System;

public class Gun : MonoBehaviour
{
    [SerializeField] GunSO gunSO;
    [Space]
    [SerializeField] float bulletSpeed;
    [Space]
    [SerializeField] Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlaresPS;

    Volume attackTimer;
    bool isShooting;

    void Awake()
    {
        attackTimer = new Volume(0, gunSO.rate);
    }

    void Update()
    {
        if (isShooting && attackTimer.Tick())
        {
            Fire();
        }
    }

    void Fire()
    {
        Bullet bullet = PoolUser.instance.bulletPool.Spawn(gunSO, bulletSpeed, muzzle);

        // FollowingTrail trail = PoolUser.instance.trailPool.Spawn(bullet.transform, muzzle);

        muzzleFlaresPS.Play();
    }

    public void ToggleShooting(bool toggle)
    {
        isShooting = toggle;

        if (!isShooting)
            attackTimer.ResetToZero();
    }
}
