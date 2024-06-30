using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
using System.Collections.Generic;

public class UpgradeScreen : View
{
    [SerializeField] UpgradeView prefabUpgradeView;
    [Space]
    [SerializeField] HorizontalLayoutGroup layout;

    List<UpgradeView> upgradeViews = new();

    void Start()
    {
        new []
        {
            PlayerCharacter.instance.upgradeDamage,
            PlayerCharacter.instance.upgradeAttackRate,
            PlayerCharacter.instance.upgradeAttackRange
        }
            .Map(up =>
            {
                UpgradeView view = Instantiate(prefabUpgradeView,
                                               default,
                                               default,
                                               layout.transform);

                upgradeViews.Add(view);

                view.Subscribe(up);
            });
    }

    public override void Show()
    {
        upgradeViews
            .GetUniqueRandoms(2)
            .Map(v => v.gameObject.SetActive(true))
            ;

        base.Show();
    }

    public override void Hide()
    {
        upgradeViews
            .Map(v => v.gameObject.SetActive(false))
            ;

        base.Hide();
    }
}
