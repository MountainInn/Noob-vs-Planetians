using HyperCasual.Runner;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class Enemy : Spawnable
{
    [Space]
    [SerializeField] Animator animator;
    [SerializeField] bool startRunning = false;
    [Space]
    [SerializeField] int moneyAmount;
    [Space]
    [SerializeField] Ragdoll ragdoll;


    public Health health;
    public Damage damage;

    Collider col;

    protected override void Awake()
    {
        base.Awake();

        col = GetComponent<Collider>();

        health = GetComponent<Health>();
        damage = GetComponent<Damage>();

        if (animator)
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

    public void __InitializeMoneyPickup(GameObject GameObject) => InitializeMoneyPickup(GameObject.GetComponent<MoneyPickup>());
    void InitializeMoneyPickup(MoneyPickup moneyPickup)
    {
        moneyPickup.SetAmount(moneyAmount);
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
