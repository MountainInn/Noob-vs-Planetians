using UnityEngine;
using YG;

public class UpgradeHold : MonoBehaviour
{
    static public UpgradeHold instance => _inst;
    static UpgradeHold _inst;
    UpgradeHold(){ _inst = this; }

    [Header("Upgrades")]
    [SerializeField]
    public Upgrade
        upgradeHealth,
        upgradeDamage,
        upgradeAttackRate,
        upgradeAttackRange;

    PlayerCharacter pc;

    void Start()
    {
        pc = PlayerCharacter.instance;

        upgradeHealth      .Inject(pc.health.Value,   l => Mathf.Pow(1.1f, l));
        upgradeDamage      .Inject(pc.damage.Value,   l => Mathf.Pow(1.1f, l));
        upgradeAttackRate  .Inject(pc.attackRate,     l => Mathf.Pow(1.1f, l));
        upgradeAttackRange .Inject(pc.attackRange,    l => Mathf.Pow(1.1f, l));

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
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
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
