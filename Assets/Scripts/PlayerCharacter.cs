using UnityEngine;
using HyperCasual.Runner;
using System;
using UniRx;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class PlayerCharacter : MonoBehaviour
{
    static public PlayerCharacter instance => _inst ??= FindObjectOfType<PlayerCharacter>();
    static PlayerCharacter _inst;

    [SerializeField] ParticleSystem onHealPS;
    [SerializeField] ParticleSystem onSufferPS;

    public Health mortal;
    public Damage damage;

    public StackedNumber attackRate = new();
    public StackedNumber attackRange = new();

    void Awake()
    {
        mortal = GetComponent<Health>();
        damage = GetComponent<Damage>();
    }

    void Start()
    {
        damage.Value.ForceRecalculate();
        attackRate.ForceRecalculate();
        attackRange.ForceRecalculate();

        // FindObjectOfType<ProgressBar>().SetVolume(mortal.Value);
    }

    public void Die()
    {
        GameManager.Instance.Lose();
    }
}
