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
    [Space]
    [SerializeField] Bullet prefabBullet;
    [SerializeField] FollowingTrail prefabFollowingTrail;

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
        Bullet bullet =
            GameObject.Instantiate(prefabBullet,
                                   muzzle.position,
                                   muzzle.rotation,
                                   null);
        // = bulletPool.Spawn()
        ;

        bullet.Initialize(gunSO.damage, gunSO.range, bulletSpeed);


        FollowingTrail trail =
            GameObject.Instantiate(prefabFollowingTrail,
                                   muzzle.position,
                                   muzzle.rotation,
                                   null);

        trail.target = bullet.transform;

        muzzleFlaresPS.Play();
    }

    public void ToggleShooting(bool toggle)
    {
        isShooting = toggle;

        if (!isShooting)
            attackTimer.ResetToZero();
    }
}


public interface ITarget
{
    void OnHit(Bullet bullet);
}
