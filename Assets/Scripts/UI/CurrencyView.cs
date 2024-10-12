using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using DG.Tweening;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] Currency currency;
    [SerializeField] bool subscribeToCurrency;
    [Space]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI label;

    CompositeDisposable disposables = new();

    void OnValidate()
    {
        if (icon == null)
        {
            icon = new GameObject("Icon").AddComponent<Image>();
            icon.transform.SetParent(transform);
        }

        if (label == null)
        {
            label = new GameObject("Label").AddComponent<TMPro.TextMeshProUGUI>();
            label.transform.SetParent(transform);
        }

        if (currency)
        {
            if (icon.sprite != currency.sprite)
                icon.sprite = currency.sprite;

            string currencyText = currency.react.Value.ToString();
            if (label.text != currencyText)
                label.text = currencyText;
        }
    }

    void Start()
    {
        if (subscribeToCurrency)
            InitAndSubscribe(currency);
    }

    public void Clear()
    {
        icon.sprite = null;
        label.text = "";
    }

    public void ClearDisposables()
    {
        disposables.Clear();
        disposables = new();
    }

    public void InitAndSubscribe(Currency currency)
    {
        Init(currency);

        currency
            .ObservePair()
            .Subscribe(p =>
            {
                DoPunchCounter(p.Previous, p.Current);
            })
            .AddTo(disposables);
    }

    Tween punchTween;
    void DoPunchCounter(int prevCoins, int newCoins)
    {
        float duration = .75f;

        DOVirtual.Int(prevCoins, newCoins, duration,
                      (val) => {
                          label.text = $"{val}";
                      }
        );
        // .OnComplete(() => GameState.Instance.LoadNextLevel());

        if (punchTween == null)
            punchTween =
                transform
                .DOPunchScale(transform.lossyScale * 1.05f, duration / 4)
                .OnKill(() => punchTween = null);
    }


    public void InitAndSubscribe(Price price)
    {
        Init(price);

        price.amount
            .result
            .Subscribe(c => label.text = price.amount.AsFloorInt().ToString())
            .AddTo(disposables);

        price
            .IsAffordableObservable()
            .Subscribe(aff =>
                       label.color = (aff) ? Color.green : Color.red)
            .AddTo(disposables);
    }

    public void Init(Currency currency)
    {
        icon.sprite = currency.sprite;
        label.text = currency.react.Value.ToString();
    }

    public void Init(Price price)
    {
        icon.sprite = price.currency.sprite;
        label.text = price.amount.AsFloorInt().ToString();
    }
}
