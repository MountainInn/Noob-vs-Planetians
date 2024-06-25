using UnityEngine;

public class Harm : WithCallbackInterface<Harm.IOnHarmCallback>
{
    [SerializeField] public StackedNumber damage;

    public void CauseHarm(Mortal mortal)
    {
        DoCallbacks(ionharm => ionharm.OnHarm(mortal));

        mortal.Suffer(this);
    }

    public interface IOnHarmCallback
    {
        void OnHarm(Mortal mortal);
    }
}
