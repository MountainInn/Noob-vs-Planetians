using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

[Serializable]
public class StackedNumber
{
    [SerializeField] float initial;
    float result;

    Dictionary<string, float> multipliers = new();
    Dictionary<string, float> addends = new();

    public StackedNumber(float initial)
    {
        this.initial = initial;
    }

    public float AsFloat() => result;
    public int AsFloorInt() => Mathf.FloorToInt(result);
    public int AsCeilInt() => Mathf.CeilToInt(result);

    public void SetMultiplierUntil(string name, float multiplier, UnityEvent until)
    {
        SetMultiplier(name, multiplier);

        until.AddListener(End);

        void End()
        {
            SetMultiplier(name, 1);
            until.RemoveListener(End);
        }
    }

    public void SetMultiplierUntil(string name, float multiplier, Action until)
    {
        SetMultiplier(name, multiplier);

        until += End;

        void End()
        {
            SetMultiplier(name, 1);
            until -= End;
        }
    }

    public void SetMultiplier(string name, float multiplier)
    {
        if (!multipliers.TryAdd(name, multiplier))
        {
            multipliers[name] = multiplier;
        }

        Recalculate();
    }


    public void SetAddendUntil(string name, float addend, UnityEvent until)
    {
        SetAddend(name, addend);

        until.AddListener(End);

        void End()
        {
            SetAddend(name, 0);
            until.RemoveListener(End);
        }
    }

    public void SetAddendUntil(string name, float addend, Action until)
    {
        SetAddend(name, addend);

        until += End;

        void End()
        {
            SetAddend(name, 0);
            until -= End;
        }
    }

    public void SetAddend(string name, float addend)
    {
        if (!addends.TryAdd(name, addend))
        {
            addends[name] = addend;
        }

        Recalculate();
    }

    void Recalculate()
    {
        result = initial;

        foreach (var (key, val) in multipliers)
        {
            result *= val;
        }

        foreach (var (key, val) in addends)
        {
            result += val;
        }
    }
}
