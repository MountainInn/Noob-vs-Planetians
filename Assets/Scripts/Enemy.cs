using HyperCasual.Runner;
using UnityEngine;
using TMPro;
using System;
using UniRx;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
[RequireComponent(typeof(Ragdoll))]
public class Enemy : Spawnable
{
    [Space]
    [SerializeField] Animator animator;
    [SerializeField] bool startRunning = false;
    [Space]
    [SerializeField] Ragdoll ragdoll;
    [Space]
    [SerializeField] TextMeshPro healthLabel;

    public Health health;
    public Damage damage;

    protected override void Awake()
    {
        base.Awake();

        health = GetComponent<Health>();
        damage = GetComponent<Damage>();

        health.Value
            .current
            .Subscribe(cur => healthLabel.text = $"{cur}")
            .AddTo(this);

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");
    }

    public override void ResetSpawnable()
    {
        ragdoll.Activate(false);

        transform.position = SavedPosition;

        health.Value.Refill();
    }

    public void __ReactOnDamage() => ReactOnDamage();
    public void ReactOnDamage()
    {
        PSMobDamaged.instance.Fire(transform.position);
    }

    public void __ActivateRagdoll() => ActivateRagdoll(true);
    public void ActivateRagdoll(bool toggle)
    {
        ragdoll.Activate(toggle);
        animator.enabled = !toggle;
    }
}
