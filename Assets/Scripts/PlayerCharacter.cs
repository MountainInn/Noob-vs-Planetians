using UnityEngine;
using HyperCasual.Runner;
using System;

[RequireComponent(typeof(Mortal))]
[RequireComponent(typeof(Harm))]
public class PlayerCharacter : MonoBehaviour, Mortal.IMortalCallback, Harm.IOnHarmCallback
{
    static public PlayerCharacter instance => _inst ??= FindObjectOfType<PlayerCharacter>();
    static PlayerCharacter _inst;

    [SerializeField] ParticleSystem onHealPS;
    [SerializeField] ParticleSystem onSufferPS;
    
    public Mortal mortal;
    public Harm harm;

    void Start()
    {
        mortal = GetComponent<Mortal>();
        harm = GetComponent<Harm>();

        var health = mortal.health;
    }

    public void OnHeal(Healing healing)
    {
        onHealPS.Play();
    }

    public void OnHarm(Mortal mortal)
    {
    }

    public void OnSuffer(Harm harm)
    {
        onSufferPS.Play();
    }

    public void OnDie()
    {
        GameManager.Instance.Lose();
    }
}
