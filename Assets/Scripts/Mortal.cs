using UnityEngine;

public class Mortal : WithCallbackInterface<Mortal.IMortalCallback>, ITarget
{
    [SerializeField] int maxHealth;

    [HideInInspector] public Volume health;

    void Awake()
    {
        health = new (maxHealth);
    }

    public void OnHit(Bullet bullet)
    {
        Suffer(bullet.harm);
    }

    public void Suffer(Harm harm)
    {
        health.Subtract(harm.damage);

        DoCallbacks(item => item.OnSuffer(harm));

        if (health.IsEmpty)
        {
            DoCallbacks(item => item.OnDie());
        }
    }

    public void Heal(Healing healing)
    {
        health.Add(healing.healing);

        DoCallbacks(item => item.OnHeal(healing));
    }

    public interface IMortalCallback
    {
        void OnHeal(Healing healing);
        void OnSuffer(Harm harm);
        void OnDie();
    }
}
