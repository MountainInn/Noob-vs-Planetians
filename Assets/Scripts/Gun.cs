using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunSO gunSO;
    [Space]
    [SerializeField] float bulletSpeed;
    [Space]
    [SerializeField] Transform muzzle;
    [Space]
    [SerializeField] Bullet prefabBullet;

    Volume attackTimer;

    void Awake()
    {
        attackTimer = new Volume(0, gunSO.rate);
    }

    void Update()
    {
        if (attackTimer.Tick())
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

        bullet.damage = gunSO.damage;

        var rb = bullet.GetComponent<Rigidbody>();

        rb.velocity = muzzle.forward * bulletSpeed;
    }
}


public interface IHitbox
{
    void OnHit(Bullet bullet);
}
