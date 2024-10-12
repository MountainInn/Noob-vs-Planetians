using UnityEngine;
using YG;
using Zenject;

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

    [Inject] void Construct(YandexSaveSystem sv)
    {
        sv.Register(
            save =>
            {
                save.healthUpgradeLevel = upgradeHealth.level.ware.L;
                save.damageUpgradeLevel = upgradeDamage.level.ware.L;
                save.attackRateUpgradeLevel = upgradeAttackRate.level.ware.L;
                save.attackRangeUpgradeLevel = upgradeAttackRange.level.ware.L;
            },
            load =>
            {
                upgradeHealth.level.ware.SetLevel(load.healthUpgradeLevel);
                upgradeDamage.level.ware.SetLevel(load.damageUpgradeLevel);
                upgradeAttackRate.level.ware.SetLevel(load.attackRateUpgradeLevel);
                upgradeAttackRange.level.ware.SetLevel(load.attackRangeUpgradeLevel);
            });
    }

    public void Initialize()
    {
        upgradeHealth.Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                             priceCalculation: l => Mathf.Pow(1.3f, l));

        upgradeDamage.Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                             priceCalculation: l => Mathf.Pow(1.3f, l));

        upgradeAttackRate.Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                                 priceCalculation: l => Mathf.Pow(1.3f, l));

        upgradeAttackRange.Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                                  priceCalculation: l => Mathf.Pow(1.3f, l));

        new[] {
            upgradeHealth,
            upgradeDamage,
            upgradeAttackRate,
            upgradeAttackRange
        }
            .ForEach(upg =>
            {
                upg.stat.ForceRecalculate();
                upg.level.onBuy += (l) =>
                {
                    WeaponExperience.instance.AddExpirience(1);
                };
            });
    }

}
