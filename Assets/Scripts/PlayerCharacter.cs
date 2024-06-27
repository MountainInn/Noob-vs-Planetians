using UnityEngine;
using HyperCasual.Runner;
using System;
using Zenject;

[RequireComponent(typeof(Mortal))]
[RequireComponent(typeof(Harm))]
public class PlayerCharacter : MonoBehaviour, Mortal.IMortalCallback, Harm.IOnHarmCallback
{
    static public PlayerCharacter instance => _inst ??= FindObjectOfType<PlayerCharacter>();
    static PlayerCharacter _inst;

    [SerializeField] ParticleSystem onHealPS;
    [SerializeField] ParticleSystem onSufferPS;

    [Inject]
    public void Construct(ProgressBar progressBar)
    {
        progressBar.SetVolume(mortal.health);
    }
    
    [Inject] public Mortal mortal;
    [Inject] public Harm harm;

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
