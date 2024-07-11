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
    [SerializeField] ProgressBar healthBar;


    public Health health;
    public Damage damage;

    Collider col;

    protected override void Awake()
    {
        base.Awake();

        col = GetComponent<Collider>();

        health = GetComponent<Health>();
        damage = GetComponent<Damage>();

        health.Volume
            .current
            .Subscribe(cur => healthLabel.text = $"{cur}")
            .AddTo(this);

        healthBar.Subscribe(gameObject, health.Volume);

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");
    }

    public override void ResetSpawnable()
    {
        ragdoll.Activate(false);

        transform.position = SavedPosition;

        health.Volume.Refill();
    }

    public void __ReactOnDamage() => ReactOnDamage();
    public void ReactOnDamage()
    {
        PSMobDamaged.instance.Fire(col.bounds.center, 3);
    }

    public void __ActivateRagdoll() => ActivateRagdoll(true);
    public void ActivateRagdoll(bool toggle)
    {
        ragdoll.Activate(toggle);
        animator.enabled = !toggle;
    }
}
