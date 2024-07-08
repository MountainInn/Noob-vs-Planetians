using System;
using UniRx;

[Serializable]
public class Price
{
    public Currency currency;
    public IntReactiveProperty cost;

    public Action<int> onPay;

    public Price (Currency currency)
        : this(currency, 0)
    {
    }
    public Price(Currency currency, int cost)
    {
        this.currency = currency;
        this.cost = new IntReactiveProperty(cost);
    }

    public bool IsAffordable()
    {
        return cost.Value <= currency.react.Value;
    }

    public IObservable<bool> IsAffordableObservable()
    {
        return
            Observable
            .CombineLatest(cost, currency.react,
                           (cost, currency) => cost <= currency);
    }

    public IObservable<float> SavingProgressObservable()
    {
        return
            Observable
            .CombineLatest(cost, currency.react,
                           (cost, currency) => (float)currency / cost);
    }


    public void Pay()
    {
        currency.react.Value -= cost.Value;
        onPay?.Invoke(cost.Value);
    }

    public void GetPaid()
    {
        currency.react.Value += cost.Value;
    }
}
