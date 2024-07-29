using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class RebirthingHealth : Health
{
    [Header("Rebirth")]
    [SerializeField] float rebirthingMult = 1f;
    [SerializeField] public UnityEvent onRebirth;

    int rebirthCount = 0;
    
    protected new void Awake()
    {
        base.Awake();

        Volume
            .ObserveIsEmpty()
            .Subscribe(empty =>
            {
                if (empty)
                    Rebirth();
            })
            .AddTo(this);
    }

    void Rebirth()
    {
        rebirthCount++;

        Value.SetMultiplier("Rebirth Mult", Mathf.Pow(rebirthingMult, rebirthCount));
        
        onRebirth?.Invoke();
    }
}
