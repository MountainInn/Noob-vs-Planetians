using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Level : Buyable<Level>.IWare
{
    [SerializeField] public Volume Volume = new();
    [SerializeField] public UnityEvent<int> onSetLevel;

    protected List<Action<int>> statCalculations;


    public int L => Mathf.FloorToInt(Volume.current.Value);

    public Level(Action<int> statsCalculation, int maximumLevel = int.MaxValue)
    {
        statCalculations = new List<Action<int>>(){
            statsCalculation
        };

        SetMaximum(maximumLevel);
    }

    public void AddCalculation(params Action<int>[] statCalculations)
    {
        this.statCalculations.AddRange(statCalculations);
    }

    public void Up()
    {
        SetLevel((int)Volume.current.Value + 1);
    }

    public void SetMaximum(int maximumLevel)
    {
        Volume.Resize(maximumLevel);
    }

    public void SetLevel(int level)
    {
        this.Volume.SetCurrent(level);

        if (statCalculations != null && statCalculations.Any())
            foreach (var item in statCalculations)
            {
                item.Invoke(level);
            }

        onSetLevel?.Invoke(level);
    }

    public IObservable<bool> ObserveIsAtMaxLevel()
    {
        return Volume.ObserveFull();
    }

    public IObservable<bool> ObserveIsAffordable()
    {
        return
            ObserveIsAtMaxLevel()
            .Select(b => !b);
    }
}
