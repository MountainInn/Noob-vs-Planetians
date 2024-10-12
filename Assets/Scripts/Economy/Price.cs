using System;
using UniRx;

[Serializable]
public class Price
{
    public Currency currency;
    public StackedNumber amount;

    public Action<int> onPay;


    public bool IsAffordable()
    {
        return amount.AsFloorInt() <= currency.react.Value;
    }

    public IObservable<bool> IsAffordableObservable()
    {
        return
            Observable
            .CombineLatest(amount.result, currency.react,
                           (cost, currency) => cost <= currency);
    }

    public IObservable<float> SavingProgressObservable()
    {
        return
            Observable
            .CombineLatest(amount.result, currency.react,
                           (cost, currency) => (float)currency / cost);
    }


    public void Pay()
    {
        int payAmount = amount.AsFloorInt();

        currency.react.Value -= payAmount;

        onPay?.Invoke(payAmount);
    }

    public void GetPaid()
    {
        currency.react.Value += amount.AsFloorInt();
    }
}
