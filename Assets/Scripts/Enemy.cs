using HyperCasual.Runner;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
[RequireComponent(typeof(Ragdoll))]
public class Enemy : MonoBehaviour
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

    void Awake()
    {
        health = GetComponent<Health>();
        damage = GetComponent<Damage>();

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");
    }

    public void __ReactOnDamage() => ReactOnDamage();
    public void ReactOnDamage()
    {
        healthLabel.text = $"{health.Value.current}";

        PSMobDamaged.instance.Fire(transform.position);
    }

    public void __ActivateRagdoll() => ActivateRagdoll();
    public void ActivateRagdoll()
    {
        animator.enabled = false;
        ragdoll.Activate(true);
    }
}
