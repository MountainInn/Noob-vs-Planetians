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

    public Upgrade()
    {

    }

    public void Inject(Func<int, float> bonusCalculation)
    {
        this.bonusCalculation = bonusCalculation;

        level.ware.Volume.maximum.Value = int.MaxValue;
        level.ware.AddCalculation(OnLevelUp);
        level.onBuy = OnBuy;
        level.prices = new List<Price>() { price };
    }

    void OnBuy(Level level)
    {
        level.Up();
    }

    void OnLevelUp(int l)
    {
        float multiplier = bonusCalculation.Invoke(l);
        stat.SetMultiplier(name, multiplier);

        price.amount.SetMultiplier(name, Mathf.Pow(1.1f, l));
    }
}
