using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    [SerializeField] public StackedNumber Value;
    [Space]
    [SerializeField] public UnityEvent onDoDamage;

    public void DoDamage(Health health)
    {
        onDoDamage?.Invoke();

        health.TakeDamage(this);
    }
}
