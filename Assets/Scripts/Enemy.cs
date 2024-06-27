using HyperCasual.Runner;
using UnityEngine;
using TMPro;
using Zenject;

[RequireComponent(typeof(Mortal))]
[RequireComponent(typeof(Obstacle))]
[RequireComponent(typeof(Ragdoll))]
public class Enemy : Spawnable, Mortal.IMortalCallback
{
    [SerializeField] Animator animator;
    [SerializeField] bool startRunning = false;
    [Space]
    [SerializeField] Ragdoll ragdoll;
    [Space]
    [SerializeField] TextMeshPro healthLabel;

    [Inject] Mortal mortal;
    [Inject] Harm harm;

    override protected void Awake()
    {
        base.Awake();

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter playerCharacter))
        {
            playerCharacter.mortal.Suffer(harm);
        }
    }

    public void OnHeal(Healing healing)
    {

    }

    public void OnSuffer(Harm harm)
    {
        healthLabel.text = $"{mortal.health.current}";

        PSMobDamaged.instance.Fire(transform.position);
    }

    public void OnDie()
    {
        animator.enabled = false;
        ragdoll.Activate();

        MoneyPS.instance.Fire(transform.position);
    }
}
