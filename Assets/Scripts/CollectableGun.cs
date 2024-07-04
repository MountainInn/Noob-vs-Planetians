using UnityEngine;

public class CollectableGun : MonoBehaviour
{
    Gun gun;

    void Awake()
    {
        gun = GetComponentInChildren<GunSlot>().RandomEquip();
    }

    public void AddGun()
    {
        GunBelt.instance.Add(gun);
    }
}
