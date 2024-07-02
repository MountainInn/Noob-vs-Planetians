using UnityEngine;


[RequireComponent(typeof(GunSlot))]
public class CollectableGun : HyperCasual.Runner.Collectable, ICollectable
{
    Gun gun;

    override protected void Awake()
    {
        base.Awake();

        gun = GetComponent<GunSlot>().RandomEquip();
    }

    public void AddGun()
    {
        GunBelt.instance.Add(gun);
    }
}
