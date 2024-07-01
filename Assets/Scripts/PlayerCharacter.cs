using UnityEngine;
using HyperCasual.Runner;
using System;
using UniRx;
using YG;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class PlayerCharacter : MonoBehaviour
{
    static public PlayerCharacter instance => _inst ??= FindObjectOfType<PlayerCharacter>();
    static PlayerCharacter _inst;

    [SerializeField] ParticleSystem onHealPS;
    [SerializeField] ParticleSystem onSufferPS;
    [Space]
    [SerializeField] EquipmentSlot weaponSlot;

    public Rigidbody rb;
    public Health mortal;
    public Damage damage;

    public StackedNumber attackRate = new();
    public StackedNumber attackRange = new();

    [SerializeField]
    public Upgrade
        upgradeHealth,
        upgradeDamage,
        upgradeAttackRate,
        upgradeAttackRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mortal = GetComponent<Health>();
        damage = GetComponent<Damage>();

        // upgradeHealth   .Inject(mortal.Value,   l => {  });
        upgradeDamage      .Inject(damage.Value,   l => l * 10);
        upgradeAttackRate  .Inject(attackRate,     l => -Mathf.Max(l, 10) * .1f);
        upgradeAttackRange .Inject(attackRange,    l => l * 2);

        damage.Value.ForceRecalculate();
        attackRate.ForceRecalculate();
        attackRange.ForceRecalculate();



        WeaponExperience.instance
            .onNewWeaponUnlocked.AddListener(() =>
            {
                SwitchWeapon();
            });
    }

    void SwitchWeapon()
    {
        weaponSlot.MaybeSwitchEquipment(
            (int)WeaponExperience.instance.expVolume.current.Value);
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
        GameManager.Instance.Lose();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    void Save()
    {
        YandexGame.savesData.healthUpgradeLevel = upgradeHealth.level.ware.L;
        YandexGame.savesData.damageUpgradeLevel = upgradeDamage.level.ware.L;
        YandexGame.savesData.attackRateUpgradeLevel = upgradeAttackRate.level.ware.L;
        YandexGame.savesData.attackRangeUpgradeLevel = upgradeAttackRange.level.ware.L;

        YandexGame.SaveProgress();
    }

    void Load()
    {
        upgradeHealth.level.ware.SetLevel(YandexGame.savesData.healthUpgradeLevel);
        upgradeDamage.level.ware.SetLevel(YandexGame.savesData.damageUpgradeLevel);
        upgradeAttackRate.level.ware.SetLevel(YandexGame.savesData.attackRateUpgradeLevel);
        upgradeAttackRange.level.ware.SetLevel(YandexGame.savesData.attackRangeUpgradeLevel);
    }
}
