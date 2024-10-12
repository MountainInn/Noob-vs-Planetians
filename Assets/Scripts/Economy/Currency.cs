using UnityEngine;
using System;
using UniRx;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Currency", menuName = "SO/Currency")]
public class Currency : ScriptableObject
{
    public Sprite sprite;
    public ReactiveProperty<int> react;
    [SerializeField] public UnityEvent<PlayerCharacter> onPlayerEnter;

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
        return react.Pairwise();
    }
}
