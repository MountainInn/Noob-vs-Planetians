using UnityEngine;
using HyperCasual.Runner;
using System;
using UniRx;
using YG;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class PlayerCharacter : MonoBehaviour
{
    static public PlayerCharacter instance => _inst;
    static PlayerCharacter _inst;
    PlayerCharacter() { _inst = this; }

    [SerializeField] ParticleSystem onSufferPS;
    [SerializeField] Animator animator;
    [Space]
    [SerializeField] public GunSlot gunSlot;
    [SerializeField] public GunBelt gunBelt;

    public Rigidbody rb;
    public Health health;
    public Damage damage;
    public StackedNumber attackRate;
    public StackedNumber attackRange;

    [Header("Upgrades")]
    [SerializeField]
    public Upgrade
        upgradeHealth,
        upgradeDamage,
        upgradeAttackRate,
        upgradeAttackRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        damage = GetComponent<Damage>();

        upgradeHealth      .Inject(health.Value,   l => l * 100 );
        upgradeDamage      .Inject(damage.Value,   l => l * 10);
        upgradeAttackRate  .Inject(attackRate,     l => -Mathf.Max(l, 10) * .1f);
        upgradeAttackRange .Inject(attackRange,    l => l * 2);


        new [] {
            upgradeHealth,
            upgradeDamage,
            upgradeAttackRate,
            upgradeAttackRange
        }
            .Map(upg => upg.level.onBuy += (l) =>
            {
                WeaponExperience.instance.AddExpirience(1);
            });


        damage.Value.ForceRecalculate();
        attackRate.ForceRecalculate();
        attackRange.ForceRecalculate();
    }

    void Start()
    {
        health.Volume.ObserveEmpty()
            .Subscribe(b =>
            {
                if (b)
                    Die();
            })
            .AddTo(this);

        FindObjectsOfType<Gun>()
            .Map(g => g.Initialize(this));

        gunSlot
            .AddListeners(gun => gun.onShot.AddListener(SetTriggerShoot),
                          gun => gun.onShot.RemoveListener(SetTriggerShoot));
       
        FullStop();
    }

    void SetTriggerShoot()
    {
        animator.SetTrigger("Shoot");
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    public void Die()
    {
        FullStop();

        GameManager.Instance.Lose();
    }

    public void FullStop()
    {
        gunSlot.GetFirstActive().ToggleShooting(false);
        gunBelt.ToggleShooting(false);

        PlayerController.Instance.enableMovement = false;
    }

    public void Ressurect()
    {
        FullForward();
    }

    public void FullForward()
    {
        gunSlot.GetFirstActive().ToggleShooting(true);
        gunBelt.ToggleShooting(true);

        PlayerController.Instance.enableMovement = true;
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void Save()
    {
        // YandexGame.savesData.healthUpgradeLevel = upgradeHealth.level.ware.L;
        YandexGame.savesData.damageUpgradeLevel = upgradeDamage.level.ware.L;
        YandexGame.savesData.attackRateUpgradeLevel = upgradeAttackRate.level.ware.L;
        YandexGame.savesData.attackRangeUpgradeLevel = upgradeAttackRange.level.ware.L;

        YandexGame.SaveProgress();
    }

    void Load()
    {
        // upgradeHealth.level.ware.SetLevel(YandexGame.savesData.healthUpgradeLevel);
        upgradeDamage.level.ware.SetLevel(YandexGame.savesData.damageUpgradeLevel);
        upgradeAttackRate.level.ware.SetLevel(YandexGame.savesData.attackRateUpgradeLevel);
        upgradeAttackRange.level.ware.SetLevel(YandexGame.savesData.attackRangeUpgradeLevel);
    }
}
