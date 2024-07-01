using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Level : Buyable<Level>.IWare
{
    public int L => Mathf.FloorToInt(level.current.Value);
    public Volume level = new();

    protected List<Action<int>> statCalculations;


    public Level(int maximumLevel, Action<int> statsCalculation)
        : this(statsCalculation)
    {
        SetMaximum(maximumLevel);
    }

    public Level(Action<int> statsCalculation)
    {
        statCalculations = new List<Action<int>>(){
            statsCalculation
        };
    }

    public void AddCalculation(params Action<int>[] statCalculations)
    {
        this.statCalculations.AddRange(statCalculations);
    }

    public void Up()
    {
        SetLevel((int)level.current.Value + 1);
    }

    public void SetMaximum(int maximumLevel)
    {
        level.Resize(maximumLevel);
    }

    public void SetLevel(int level)
    {
        this.level.current.Value = level;

        foreach (var item in statCalculations)
        {
            item.Invoke(level);
        }
    }

    public IObservable<bool> ObserveIsAtMaxLevel()
    {
        return level.ObserveFull();
    }

    public IObservable<bool> ObserveIsAffordable()
    {
        return
            ObserveIsAtMaxLevel()
            .Select(b => !b);
    }
}
