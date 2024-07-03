using UnityEngine;
using UnityEngine.Events;

public class Healing : MonoBehaviour
{
    [SerializeField] public int Value;
    [Space]
    [SerializeField] public UnityEvent onHeal;

    public void __Heal(PlayerCharacter pc) => Heal(pc.health);
    public void __Heal(Health mortal) => Heal(mortal);
    public void Heal(Health mortal)
    {
        onHeal?.Invoke();

        mortal.Heal(this);
    }
}
