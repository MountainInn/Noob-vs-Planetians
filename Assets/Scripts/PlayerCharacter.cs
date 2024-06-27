using UnityEngine;
using HyperCasual.Runner;
using System;
using Zenject;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class PlayerCharacter : MonoBehaviour
{
    static public PlayerCharacter instance => _inst ??= FindObjectOfType<PlayerCharacter>();
    static PlayerCharacter _inst;

    [SerializeField] ParticleSystem onHealPS;
    [SerializeField] ParticleSystem onSufferPS;

    [Inject]
    public void Construct(ProgressBar progressBar)
    {
        progressBar.SetVolume(mortal.Value);
    }
    
    [Inject] public Health mortal;
    [Inject] public Damage harm;

    public void Die()
    {
        GameManager.Instance.Lose();
    }
}
