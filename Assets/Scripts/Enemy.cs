using HyperCasual.Runner;
using UnityEngine;
using TMPro;
using Zenject;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
[RequireComponent(typeof(Obstacle))]
[RequireComponent(typeof(Ragdoll))]
public class Enemy : Spawnable
{
    [SerializeField] Animator animator;
    [SerializeField] bool startRunning = false;
    [Space]
    [SerializeField] Ragdoll ragdoll;
    [Space]
    [SerializeField] TextMeshPro healthLabel;

    [Inject] Health mortal;
    [Inject] Damage harm;

    override protected void Awake()
    {
        base.Awake();

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");
    }

    public void TakeDamage()
    {
        healthLabel.text = $"{mortal.Value.current}";

        PSMobDamaged.instance.Fire(transform.position);
    }

    public void Die()
    {
        animator.enabled = false;
        ragdoll.Activate();
    }
}
