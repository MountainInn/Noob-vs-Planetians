using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    [SerializeField] public StackedNumber Value;
    [Space]
    [SerializeField] public UnityEvent onDoDamage;

    public void __DoDamage(PlayerCharacter PlayerCharacter) => DoDamage(PlayerCharacter.mortal);
    public void __DoDamage(Enemy enemy) => DoDamage(enemy.health);
    public void __DoDamage(Health health) => DoDamage(health);
    public void DoDamage(Health health)
    {
        onDoDamage?.Invoke();

        health.TakeDamage(this);
    }
}
