using UnityEngine;
using System;
using UniRx;

[System.SerializableAttribute]
public class Volume
{
    public FloatReactiveProperty maximum => _maximum ??= new();
    public FloatReactiveProperty current => _current ??= new();

    [SerializeField] protected FloatReactiveProperty _current, _maximum;

    public float Unfilled => maximum.Value - current.Value;
    public int CurrentInt => (int)current.Value;


    public Volume() {}
    public Volume(float currentAndMaximum)
        :this(currentAndMaximum, currentAndMaximum)
    {

    }

    public Volume(float current, float maximum)
    {
        this._current = new FloatReactiveProperty(current);
        this._maximum = new FloatReactiveProperty(maximum);
    }

    public IObservable<(float current, float maximum, float ratio)> ObserveAll() =>
        Observable.CombineLatest(
            current, maximum,
            (cur , max) => (cur, max, Ratio));

    public IObservable<float> ObserveRatio() =>
        current
        .Select(_ => ((float.IsNaN(Ratio)) ? 0f : Ratio));

    public IObservable<bool> ObserveFull() =>
        Observable.CombineLatest(
            current, maximum,
            (cur , max) => IsFull);

    public IObservable<bool> ObserveIsEmpty() =>
        current
        .Select(v => IsEmpty);

    public IObservable<bool> ObserveRefill() =>
        ObserveFull()
        .Pairwise()
        .Select(fulls =>
                fulls.Previous == false && fulls.Current == true);

    public IObservable<float> ObserveChange() =>
        current
        .Pairwise((a, b) => b - a);

    public bool IsFull => (current.Value == maximum.Value);
    public bool IsEmpty => (current.Value == 0);

    public void ResetToZero()
    {
        current.Value = 0;
    }

    public float Ratio
    {
        get {
            float ratio = (current.Value / maximum.Value);

            if (float.IsNaN(ratio))
                ratio = 0;

            return ratio;
        }
    }

    public void ResetTo(float newCurrentAmount)
    {
        current.Value = newCurrentAmount;
    }

    public bool Tick()
    {
        bool result = Add(Time.deltaTime);

        if (result)
            current.Value = 0;

        return result;
    }

    public bool SetCurrent(float val)
    {
        current.Value = Mathf.Clamp(val, 0, maximum.Value);

        return current.Value >= maximum.Value;
    }

    public bool Add(float amount, out float overflow)
    {
        overflow = 0;

        if (amount > Unfilled)
            overflow = amount - Unfilled;

        return Add(amount);
    }
    public bool Add(float amount)
    {
        amount = Mathf.Min(amount, Unfilled);
        current.Value = Mathf.Clamp(current.Value + amount,
                                    0, maximum.Value);

        return current.Value >= maximum.Value;
    }

    public bool Subtract(float amount, out float overflow)
    {
        overflow = 0;

        if (amount > current.Value)
            overflow = amount - current.Value;

        return Subtract(amount);
    }

    public bool Subtract(float amount)
    {
        current.Value -= Mathf.Min(amount, current.Value);

        return current.Value <= 0;
    }

    public void Reinitialize(float current, float maximum)
    {
        this.current.Value = current;
        this.maximum.Value = maximum;
    }

    public void ResizeAndRefill(float newMaximum)
    {
        maximum.Value = newMaximum;
        current.Value = newMaximum;
    }

    public void Resize(float newMaximum)
    {
        maximum.Value = newMaximum;
        current.Value = Clamp(current.Value);
    }

    public void Refill()
    {
        current.Value = maximum.Value;
    }

    float Clamp(float amount)
    {
        return Mathf.Clamp(amount, 0, maximum.Value);
    }

    public override string ToString()
    {
        return $"{current.Value:0}/{maximum.Value:0}";
    }
}
