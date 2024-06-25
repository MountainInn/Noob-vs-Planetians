using UnityEngine;

public class Healing : WithCallbackInterface<Healing.IOnHealCallback>
{
    [SerializeField] [Min(1)] public int healing;

    public void Heal(Mortal mortal)
    {
        DoCallbacks(ionheal => ionheal.OnHeal(mortal));

        mortal.Heal(this);
    }

    public interface IOnHealCallback
    {
        void OnHeal(Mortal mortal);
    }
}
