using UnityEngine;
using System;
using UniRx;

[CreateAssetMenu(fileName = "Currency", menuName = "SO/Currency")]
public class Currency : ScriptableObject
{
    public Sprite sprite;
    public ReactiveProperty<int> value;

    public IObservable<int> ObserveAddition()
    {
        return
            ObserveChange()
            .Where(change => change > 0);
    }

    public IObservable<int> ObserveSubtraction()
    {
        return
            ObserveChange()
            .Where(change => change <= 0);
    }

    public IObservable<int> ObserveChange()
    {
        return
            ObservePair()
            .Select(pair => pair.Current - pair.Previous);
    }

    public IObservable<Pair<int>> ObservePair()
    {
        return value.Pairwise();
    }
}
