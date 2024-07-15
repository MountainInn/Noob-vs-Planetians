using UnityEngine;
using HyperCasual.Runner;
using System;
using UniRx;
using YG;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Damage))]
public class PlayerCharacter : MonoBehaviour
{
    static public PlayerCharacter instance => _inst;
    static PlayerCharacter _inst;
    PlayerCharacter() { _inst = this; }

    [SerializeField] ParticleSystem onSufferPS;
    [SerializeField] Animator animator;
    [Space]
    [SerializeField] public GunSlot gunSlot;
    [SerializeField] public GunBelt gunBelt;

    public Rigidbody rb;
    public Health health;
    public Damage damage;
    public StackedNumber attackRate;
    public StackedNumber attackRange;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        damage = GetComponent<Damage>();


        damage.Value.ForceRecalculate();
        attackRate.ForceRecalculate();
        attackRange.ForceRecalculate();
    }

    void Start()
    {
        health.Volume.ObserveEmpty()
            .Subscribe(b =>
            {
                if (b)
                    Die();
            })
            .AddTo(this);

        FindObjectsOfType<Gun>(true)
            .ForEach(g => g.Initialize(this));

        gunSlot.MaybeSwitchEquipment(WeaponExperience.instance.currentLevel);

        FullStop();
    }

    public void RefillHealth()
    {
        health.Volume.Refill();
    }

    public void Die()
    {
        FullStop();

        GameManager.Instance.Lose();
    }

    public void FullStop()
    {
        gunSlot.GetFirstActive().ToggleShooting(false);
        gunBelt.ToggleShooting(false);

        PlayerController.Instance.enableMovement = false;
    }

    public void Resurrect()
    {
        RefillHealth();

        FullForward();
    }

    public void FullForward()
    {
        gunSlot.GetFirstActive().ToggleShooting(true);
        gunBelt.ToggleShooting(true);

        PlayerController.Instance.enableMovement = true;
    }
}
