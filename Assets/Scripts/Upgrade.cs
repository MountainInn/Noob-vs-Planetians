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

    [NonSerialized] public StackedNumber stat;
    [NonSerialized] public Buyable<Level> level;

    Func<int,float> bonusCalculation;

    public Upgrade()
    {

    }

    public void Inject(StackedNumber stat, Func<int, float> bonusCalculation)
    {
        this.stat = stat;
        this.bonusCalculation = bonusCalculation;

        level = new Buyable<Level>(new(OnLevelUp),
                                   OnBuy,
                                   price);
    }

    void OnBuy(Level level)
    {
        level.Up();
    }

    void OnLevelUp(int l)
    {
        stat.SetAddend(name, bonusCalculation.Invoke(l));

        price.cost.Value = l * 100;
    }
}
