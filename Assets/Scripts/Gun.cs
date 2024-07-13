using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;
using UniRx;
using Cysharp.Threading.Tasks;

public class Gun : MonoBehaviour
{
    [SerializeField] public IntReactiveProperty damage;
    [SerializeField] public FloatReactiveProperty rate;
    [SerializeField] public IntReactiveProperty range;
    [SerializeField] public float bulletSpeed;
    [Space]
    [SerializeField] Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlaresPS;
    [Space]
    [SerializeField] public UnityEvent onShot;

    Volume attackTimer;
    public bool isShooting;

    PlayerCharacter player;

    [HideInInspector] public int totalDamage, totalRange;

    public void Initialize(PlayerCharacter playerCharacter)
    {
        player = playerCharacter;

        attackTimer = new Volume(0, 1);

        Observable
            .CombineLatest(player.attackRate.result, rate,
                           (playerRate, rate) => playerRate * rate)
            .Subscribe(r =>
                       attackTimer.Resize(RateToSeconds(r)))
            .AddTo(this);

        Observable
            .CombineLatest(player.damage.Value.result, damage,
                           (pdamage, damage) => pdamage + damage)
            .Subscribe(d =>
                       totalDamage = Mathf.FloorToInt(d))
            .AddTo(this);

        Observable
            .CombineLatest(player.attackRange.result, range,
                           (prange, range) => prange + range)
            .Subscribe(r =>
                       totalRange = Mathf.FloorToInt(r))
            .AddTo(this);
    }

    float RateToSeconds(float rate) => 10f / rate;

    void Update()
    {
        if (player != null && isShooting && attackTimer.Tick())
        {
            Fire();
        }
    }

    void Fire()
    {
        Bullet bullet = PoolUser.instance.bulletPool.Spawn(this, muzzle);

        muzzleFlaresPS.Play();

        onShot?.Invoke();
    }

    public void ToggleShooting(bool toggle)
    {
        isShooting = toggle;

        if (!isShooting)
            attackTimer.ResetToZero();
    }
}
