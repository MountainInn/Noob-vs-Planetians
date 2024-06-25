using HyperCasual.Runner;
using UnityEngine;

[RequireComponent(typeof(Mortal))]
[RequireComponent(typeof(Obstacle))]
[RequireComponent(typeof(Ragdoll))]
public class Enemy : Spawnable, Mortal.IMortalCallback
{
    [SerializeField] Animator animator;
    [SerializeField] bool startRunning = false;
    [Space]
    [SerializeField] Ragdoll ragdoll;

    Mortal mortal;

    override protected void Awake()
    {
        base.Awake();

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");

        mortal = GetComponent<Mortal>();
    }

    public void OnHeal(Healing healing)
    {

    }

    public void OnSuffer(Harm harm)
    {
        Debug.Log($"Add Blood Splatters");
    }

    public void OnDie()
    {
        animator.enabled = false;
        ragdoll.Activate();

        MoneyPS.instance.Fire(transform.position);
    }
}
