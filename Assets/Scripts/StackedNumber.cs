using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UniRx;

[Serializable]
public class StackedNumber
{
    [SerializeField] public float initial;
    [SerializeField] public FloatReactiveProperty result = new();
    [SerializeField] public List<float> serializedAddends = new();
    [SerializeField] public List<float> serializedMultipliers = new();

    public Action onRecalculated;
   

    Dictionary<string, float> multipliers = new();
    Dictionary<string, float> addends = new();

    bool recalculated = false;

    public StackedNumber()
    {
    }
    public StackedNumber(float initial)
    {
        this.initial = initial;
    }

    public void ForceRecalculate()
    {
        recalculated = false;
        MaybeRecalculate();
    }

    public void SetInitial(float initial)
    {
        this.initial = initial;
        recalculated = false;
        MaybeRecalculate();
    }

    public int AsFloorInt() => Mathf.FloorToInt(AsFloat());
    public int AsCeilInt() => Mathf.CeilToInt(AsFloat());

    public float AsFloat()
    {
        MaybeRecalculate();
        return result.Value;
    }

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

        recalculated = false;
        MaybeRecalculate();
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

        recalculated = false;
        MaybeRecalculate();
    }

    void MaybeRecalculate()
    {
        if (recalculated)
            return;

        float tempResult = initial;

        foreach (var val in serializedMultipliers)
        {
            tempResult *= val;
        }
        foreach (var (key, val) in multipliers)
        {
            tempResult *= val;
        }

        foreach (var val in serializedAddends)
        {
            tempResult += val;
        }
        foreach (var (key, val) in addends)
        {
            tempResult += val;
        }

        result.Value = tempResult;
    }

    public StatMutation GetModifier(string name)
    {
        multipliers.TryAdd(name, 1);

        return new StatMutation(multipliers, name);
    }

    public struct StatMutation
    {
        private float val;

        Dictionary<string, float> dict;
        string key;

        public StatMutation(Dictionary<string, float> dict, string key)
        {
            this.dict = dict;
            this.key = key;

            this.val = dict[key];
        }

        public StatMutation Set(float value)
        {
            val = value;

            return this;
        }

        public StatMutation Until(UnityEvent until)
        {
            until.AddListener(Reset);
            until.AddListener(UnSub);

            void UnSub()
            {
                until.RemoveListener(Reset);
                until.RemoveListener(UnSub);
            }

            return this;
        }

        void Reset()
        {
            dict[key] = 1;
        }

        public StatMutation Add(float addition)
        {
            val += addition;

            return this;
        }

        public StatMutation Clamp(float min, float max)
        {
            val = Mathf.Clamp(val, min, max);

            return this;
        }

        public StatMutation Log()
        {
            Debug.Log($"{key}: {val}");

            return this;
        }

        public void Apply()
        {
            dict[key] = this.val;
        }
    }
}
