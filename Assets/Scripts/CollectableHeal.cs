using UnityEngine;

[RequireComponent(typeof(Healing))]
public class CollectableHeal : HyperCasual.Runner.Collectable, ICollectable
{
    [SerializeField] ParticleSystem onHealPickupPS;

    public Healing healing;

    override protected void Awake()
    {
        base.Awake();

        healing = GetComponent<Healing>();
    }

    public void AddGun()
    {
        healing.Heal(PlayerCharacter.instance.health);

        GameObject.Destroy(gameObject);
    }
}
