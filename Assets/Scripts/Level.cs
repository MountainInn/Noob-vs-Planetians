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

    public void SetMaximum(int maximumLevel)
    {
        Volume.Resize(maximumLevel);
    }

    public void Up()
    {
        SetLevel(Volume.CurrentInt + 1);
    }

    public void SetLevel(int level)
    {
        int cachedLevel = Volume.CurrentInt;

        Volume.SetCurrent(level);

        if (cachedLevel == Volume.CurrentInt)
            return;

        if (statCalculations != null && statCalculations.Any())
        {
            foreach (var item in statCalculations)
            {
                item.Invoke(Volume.CurrentInt);
            }
        }

        onSetLevel?.Invoke(Volume.CurrentInt);
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
