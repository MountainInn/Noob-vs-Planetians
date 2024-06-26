using UnityEngine;


[RequireComponent(typeof(EquipmentSlot))]
public class CollectableGun : HyperCasual.Runner.Collectable, ICollectable
{
    Gun gun;

    override protected void Awake()
    {
        base.Awake();

        gun =
            GetComponent<EquipmentSlot>()
            .GetFirstActive()
            ?.GetComponent<Gun>();
    }

    public void OnCollect()
    {
        GunBelt.instance.Add(gun);
    }
}
