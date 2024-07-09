using UnityEngine;
using System.Linq;
using HyperCasual.Core;
using HyperCasual.Runner;
using HyperCasual.Gameplay;

public class FinishMult : MonoBehaviour
{
    static public FinishMult instance => _inst;
    static FinishMult _inst;
    FinishMult(){ _inst = this; }

    public int currentMultiplier {get; private set;}
    public bool hasReachedFinish {get; private set;}

    void Awake()
    {
        FinishStep.onStepped += (mult) =>
        {
            hasReachedFinish = true;
            currentMultiplier = mult;
        };
    }

    public void Reset()
    {
        hasReachedFinish = false;
        currentMultiplier = 1;
    }
}
