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

    [SerializeField] Volume attackTimer = new(0,1);
    public bool isShooting;

    PlayerCharacter player;

    [HideInInspector] public int totalDamage, totalRange;

    public void Initialize(PlayerCharacter playerCharacter)
    {
        player = playerCharacter;

        var upgradeHold = FindObjectOfType<UpgradeHold>();

        Observable
            .CombineLatest(upgradeHold.upgradeAttackRate.stat.result, rate,
                           (playerRate, rate) => playerRate / 2f + rate)
            .Subscribe(r =>
                       attackTimer.Resize(Mathf.Max(RateToSeconds(r), .1f)))
            .AddTo(this);

        Observable
            .CombineLatest(player.damage.Value.result, damage,
                           (pdamage, damage) => pdamage + damage)
            .Subscribe(d =>
                       totalDamage = Mathf.FloorToInt(d))
            .AddTo(this);

        Observable
            .CombineLatest(upgradeHold.upgradeAttackRange.stat.result, range,
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
