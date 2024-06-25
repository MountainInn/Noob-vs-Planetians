using UnityEngine;

[RequireComponent(typeof(Gun))]
public class CollectableGun : HyperCasual.Runner.Collectable, ICollectable
{
    Gun gun;

    override protected void Awake()
    {
        base.Awake();

        gun = GetComponent<Gun>();
    }

    public void OnCollect()
    {
        GunBelt.instance.Add(gun);
    }
}
