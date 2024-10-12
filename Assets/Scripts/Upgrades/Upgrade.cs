using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using HyperCasual.Core;

[Serializable]
public class Upgrade
{
    [SerializeField] public string name;
    [SerializeField] public Sprite icon;
    [SerializeField] public Price price;
    [SerializeField] public StackedNumber stat;
    [SerializeField] public Buyable<Level> level;

    Func<int,float> bonusCalculation;
    Func<int,float> priceCalculation;

    public Upgrade()
    {

    }

    public void Inject(Func<int, float> bonusCalculation,
                       Func<int, float> priceCalculation)
    {
        this.bonusCalculation = bonusCalculation;
        this.priceCalculation = priceCalculation;

        level.ware.Volume.maximum.Value = int.MaxValue;
        level.ware.AddCalculation(OnLevelUp);
        level.onBuy = OnBuy;
        level.prices = new List<Price>() { price };
    }

    public float GetIncrease()
    {
        return
            bonusCalculation.Invoke(level.ware.L + 1)
            -
            bonusCalculation.Invoke(level.ware.L);
    }

    void OnBuy(Level level)
    {
        level.Up();
    }

    void OnLevelUp(int l)
    {
        float multiplier = bonusCalculation.Invoke(l);
        stat.SetMultiplier(name, multiplier);

        float priceMult = priceCalculation.Invoke(l);
        price.amount.SetMultiplier(name, priceMult);
    }
}
