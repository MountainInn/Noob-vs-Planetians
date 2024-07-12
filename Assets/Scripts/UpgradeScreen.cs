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
            UpgradeHold.instance.upgradeHealth,
            UpgradeHold.instance.upgradeDamage,
            UpgradeHold.instance.upgradeAttackRate,
            UpgradeHold.instance.upgradeAttackRange,
        }
            .ForEach(up =>
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
