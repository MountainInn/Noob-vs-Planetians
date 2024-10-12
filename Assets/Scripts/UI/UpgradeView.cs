using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI labelDescription;
    [SerializeField] TextMeshProUGUI labelCurrentStatValue;
    [Space]
    [SerializeField] CurrencyView currencyView;
    [Space]
    [SerializeField] RectTransform adView;
    [Space]
    [SerializeField] Image statIcon;
    [Space]
    [SerializeField] Button selfButton;

    public void Subscribe(Upgrade upgrade)
    {
        statIcon.sprite = upgrade.icon;
        labelDescription.text = upgrade.name;

        currencyView.InitAndSubscribe(upgrade.price);

        upgrade.stat.result
            .Subscribe(_ => labelCurrentStatValue.text = $"+{upgrade.GetIncrease() * 10:F0}")
            .AddTo(this);

        upgrade.price
            .IsAffordableObservable()
            .Subscribe(isAffordable =>
            {
                adView.gameObject.SetActive(!isAffordable);
                currencyView.gameObject.SetActive(isAffordable);
            })
            .AddTo(this);

        selfButton.onClick.AddListener(() =>
        {
            if (upgrade.level.IsAffordable())
            {
                upgrade.level.Buy();
            }
            else
            {
                RewardDispenser.instance.ShowFreeUpgrade(upgrade);
            }

            transform.DOPunchScale(Vector3.one * 0.1f, .15f);
        });
    }
}
