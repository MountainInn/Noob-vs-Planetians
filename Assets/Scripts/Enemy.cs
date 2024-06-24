using HyperCasual.Runner;
using UnityEngine;

[RequireComponent(typeof(Obstacle))]
public class Enemy : Spawnable, IHitbox
{
    [SerializeField] [Min(1)] int maxHealth = 1;
    [Space]
    [SerializeField] Animator animator;
    [SerializeField] bool startRunning = false;
    [Space]
    [SerializeField] Ragdoll ragdoll;

    Volume health;

    override protected void Awake()
    {
        base.Awake();
       
        health = new (maxHealth);

        if (startRunning)
            animator.SetTrigger("run");
        else
            animator.SetTrigger("idle");
    }

    public void OnHit(Bullet bullet)
    {
        if (health.Subtract(bullet.damage))
        {
            Die();
        }
    }

    void Die()
    {
        animator.enabled = false;
        ragdoll.Activate();

        MoneyPS.instance.Fire(transform.position);
    }
}
