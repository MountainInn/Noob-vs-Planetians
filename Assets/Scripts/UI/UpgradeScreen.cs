using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
using System.Collections.Generic;
using HyperCasual.Runner;
using HyperCasual.Gameplay;
using System;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class UpgradeScreen : View
{
    static public UpgradeScreen instance => _inst ??= FindObjectOfType<UpgradeScreen>();
    static UpgradeScreen _inst;

    [SerializeField] public UnityEvent onStartLevelClicked;
    [Space]
    [SerializeField] UpgradeView prefabUpgradeView;
    [Space]
    [SerializeField] LayoutGroup layout;
    [SerializeField] UpgradeView healthUpgradeView;
    [SerializeField] UpgradeView damageUpgradeView;
    [SerializeField] UpgradeView attackRateUpgradeView;
    [SerializeField] UpgradeView attackRangeUpgradeView;
    [Space]
    [SerializeField] public Button startLevelButton;

    List<UpgradeView> upgradeViews = new();

    void Awake()
    {
        startLevelButton.onClick.AddListener(() => onStartLevelClicked?.Invoke());
    }

    public void OtherInitialize()
    {
        new []
        {
            (UpgradeHold.instance.upgradeHealth, healthUpgradeView),
            (UpgradeHold.instance.upgradeDamage, damageUpgradeView),
            (UpgradeHold.instance.upgradeAttackRate, attackRateUpgradeView),
            (UpgradeHold.instance.upgradeAttackRange, attackRangeUpgradeView),
        }
            .ForEach(tuple =>
            {
                var (upgrade, view) = tuple;
                
                view.transform.localPosition = view.transform.localPosition.WithZ(0);

                upgradeViews.Add(view);

                view.Subscribe(upgrade);
            });
    }

    public override void Show()
    {
        upgradeViews
            .ForEach(v => v.gameObject.SetActive(true))
            ;

        base.Show();
    }

    public override void Hide()
    {
        upgradeViews
            .ForEach(v => v.gameObject.SetActive(false))
            ;

        base.Hide();
    }
}
