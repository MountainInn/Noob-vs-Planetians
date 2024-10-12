using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class Buyable<T>
    where T : Buyable<T>.IWare
{
    [SerializeField] public T ware;
    [SerializeField] public List<Price> prices;

    public Action<T> onBuy;

    public Buyable()
    {
        
    }
    public Buyable(T ware, Action<T> onBuy, params Price[] prices)
    {
        this.prices = prices.ToList();
        this.ware = ware;
        this.onBuy = onBuy;
    }

    public bool IsAffordable()
    {
        return prices.All(price => price.IsAffordable());
    }

    public IObservable<bool> ObserveIsAffordable()
    {
        return
            Observable
            .CombineLatest(
                prices
                .Select(price => price.IsAffordableObservable())
                .Append(ware.ObserveIsAffordable())
            )
            .Select(affordables => affordables.All(a => a == true));
    }

    public IObservable<float> SavingProgressObservable()
    {
        return
            Observable
            .CombineLatest(
                prices
                .Select(price => price.SavingProgressObservable())
            )
            .Select(savings =>
                    savings.Sum() / prices.Count);
    }

    public void Buy()
    {
        prices.ForEach(price => price.Pay());
        onBuy?.Invoke(ware);
    }

    public void Steal()
    {
        onBuy?.Invoke(ware);
    }

    public interface IWare
    {
        public IObservable<bool> ObserveIsAffordable();
    }
}
