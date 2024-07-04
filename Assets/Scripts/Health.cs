using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] [Min(1)] int maxHealth;
    [Space]
    [SerializeField] public UnityEvent onHeal;
    [SerializeField] public UnityEvent onTakeDamage;
    [SerializeField] public UnityEvent onDie;

    [HideInInspector] public Volume Value;

    void Awake()
    {
        Value = new (maxHealth);
    }

    public void __TakeDamage(Bullet bullet) => TakeDamage(bullet.damage);
    public void __TakeDamage(Enemy enemy) => TakeDamage(enemy.damage);
    public void __TakeDamage(Damage harm) => TakeDamage(harm);
    public void TakeDamage(Damage harm)
    {
        if (Value.IsEmpty)
            return;

        Value.Subtract(harm.Value.AsFloorInt());

        onTakeDamage?.Invoke();

        if (Value.IsEmpty)
        {
            onDie?.Invoke();
        }
    }

    public void __Heal(Healing healing) => Heal(healing);
    public void Heal(Healing healing)
    {
        Value.Add(healing.Value);

        onHeal?.Invoke();
    }
}
