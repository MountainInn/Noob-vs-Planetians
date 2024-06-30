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

    public Rigidbody rb;
    public Health mortal;
    public Damage damage;

    public StackedNumber attackRate = new();
    public StackedNumber attackRange = new();

    [SerializeField]
    public Upgrade
        upgradeHealth,
        upgradeDamage,
        upgradeAttackRate,
        upgradeAttackRange;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mortal = GetComponent<Health>();
        damage = GetComponent<Damage>();

        // upgradeHealth   .Inject(mortal.Value,   l => {  });
        upgradeDamage      .Inject(damage.Value,   l => l * 10);
        upgradeAttackRate  .Inject(attackRate,     l => -Mathf.Max(l, 10) * .1f);
        upgradeAttackRange .Inject(attackRange,    l => l * 2);

        damage.Value.ForceRecalculate();
        attackRate.ForceRecalculate();
        attackRange.ForceRecalculate();
    }

    public void Die()
    {
        GameManager.Instance.Lose();
    }
}
