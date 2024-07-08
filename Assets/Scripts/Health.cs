using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] public StackedNumber Value;
    [Space]
    [SerializeField] public UnityEvent onHeal;
    [SerializeField] public UnityEvent onTakeDamage;
    [SerializeField] public UnityEvent onDie;

    [HideInInspector] public Volume Volume;


    void Awake()
    {
        Volume = new (Value.initial);

        Value.onRecalculated += () =>
        {
            if (Flow.instance.currentBranch == Flow.Branch.Preparation)
                Volume.ResizeAndRefill(Value.result.Value);
            else
                Volume.Resize(Value.result.Value);
        };
    }

    public void __TakeDamage(Bullet bullet) => TakeDamage(bullet.damage);
    public void __TakeDamage(Enemy enemy) => TakeDamage(enemy.damage);
    public void __TakeDamage(Damage harm) => TakeDamage(harm);
    public void TakeDamage(Damage harm)
    {
        if (Volume.IsEmpty)
            return;

        Volume.Subtract(harm.Value.AsFloorInt());

        onTakeDamage?.Invoke();

        if (Volume.IsEmpty)
        {
            onDie?.Invoke();
        }
    }

    public void __Heal(Healing healing) => Heal(healing);
    public void Heal(Healing healing)
    {
        Volume.Add(healing.Value);

        onHeal?.Invoke();
    }
}
