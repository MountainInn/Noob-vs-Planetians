using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] [Min(1)] int maxHealth;
    [Space]
    [SerializeField] UnityEvent onHeal;
    [SerializeField] UnityEvent onTakeDamage;
    [SerializeField] UnityEvent onDie;

    [HideInInspector] public Volume Value;

    void Awake()
    {
        Value = new (maxHealth);
    }

    public void TakeDamage(Damage harm)
    {
        Value.Subtract(harm.Value.AsFloorInt());

        onTakeDamage?.Invoke();

        if (Value.IsEmpty)
        {
            onDie?.Invoke();
        }
    }

    public void Heal(Healing healing)
    {
        Value.Add(healing.Value);

        onHeal?.Invoke();
    }
}
