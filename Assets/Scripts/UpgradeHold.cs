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
        //         sv.Register(
        //             save => {
        // // YandexGame.savesData.healthUpgradeLevel = upgradeHealth.level.ware.L;
        //                 YandexGame.savesData.damageUpgradeLevel = upgradeDamage.level.ware.L;
        //                 YandexGame.savesData.attackRateUpgradeLevel = upgradeAttackRate.level.ware.L;
        //                 YandexGame.savesData.attackRangeUpgradeLevel = upgradeAttackRange.level.ware.L;
                
        //             },
        //             load => {
        // // upgradeHealth.level.ware.SetLevel(YandexGame.savesData.healthUpgradeLevel);
        //                 upgradeDamage.level.ware.SetLevel(YandexGame.savesData.damageUpgradeLevel);
        //                 upgradeAttackRate.level.ware.SetLevel(YandexGame.savesData.attackRateUpgradeLevel);
        //                 upgradeAttackRange.level.ware.SetLevel(YandexGame.savesData.attackRangeUpgradeLevel);
        //             });
    }

    public void Initialize()
    {
        upgradeHealth      .Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                                   priceCalculation: l => Mathf.Pow(1.3f, l));

        upgradeDamage      .Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                                   priceCalculation: l => Mathf.Pow(1.3f, l));
        
        upgradeAttackRate  .Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                                   priceCalculation: l => Mathf.Pow(1.3f, l));

        upgradeAttackRange .Inject(bonusCalculation: l => Mathf.Pow(1.1f, l),
                                   priceCalculation: l => Mathf.Pow(1.3f, l));

        new [] {
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
