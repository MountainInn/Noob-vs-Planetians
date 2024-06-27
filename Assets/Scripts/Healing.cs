using UnityEngine;
using UnityEngine.Events;

public class Healing : MonoBehaviour
{
    [SerializeField] public int Value;
    [Space]
    [SerializeField] public UnityEvent onHeal;

    public void Heal(Health mortal)
    {
        onHeal?.Invoke();

        mortal.Heal(this);
    }
}
