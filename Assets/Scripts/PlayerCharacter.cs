using UnityEngine;
using HyperCasual.Runner;
using System;
using Zenject;
using UniRx;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class PlayerCharacter : MonoBehaviour
{
    static public PlayerCharacter instance => _inst ??= FindObjectOfType<PlayerCharacter>();
    static PlayerCharacter _inst;

    [SerializeField] ParticleSystem onHealPS;
    [SerializeField] ParticleSystem onSufferPS;

    [Inject] public Health mortal;
    [Inject] public Damage damage;
    [Inject] void Construct(ProgressBar progressBar)
    {
        progressBar.SetVolume(mortal.Value);
    }

    public StackedNumber attackRate = new();
    public StackedNumber attackRange = new();

    void Start()
    {
        damage.Value.ForceRecalculate();
        attackRate.ForceRecalculate();
        attackRange.ForceRecalculate();
    }

    public void Die()
    {
        GameManager.Instance.Lose();
    }
}
