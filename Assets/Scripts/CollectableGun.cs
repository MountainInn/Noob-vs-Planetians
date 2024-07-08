using UnityEngine;

public class CollectableGun : MonoBehaviour
{
    Gun gun;

    void Start()
    {
        gun = GetComponentInChildren<GunSlot>().RandomEquip();
    }

    public void AddGun()
    {
        GunBelt.instance.Add(gun);
    }
}
