using System.Collections.Generic;
using UnityEngine;

public class GunBelt : MonoBehaviour
{
    static public GunBelt instance => _inst;
    static GunBelt _inst;
    GunBelt() { _inst = this; }

    [SerializeField] [Min(1)] float radius;
    [SerializeField] float rotationAnglePerSecond = 60;
    [Space]
    [SerializeField] List<Gun> initialGuns;

    List<Gun> guns = new();

    float angleStep = 0;

    void Awake()
    {
        foreach (var item in initialGuns)
            Add(item);
    }

    public void Reset()
    {
        guns.DestroyAll();
    }

    public void ToggleShooting(bool toggle)
    {
        foreach (var item in guns)
            item.ToggleShooting(toggle);
    }

    public void Add(Gun gun)
    {
        guns.Add(gun);

        gun.transform.SetParent(transform);
        gun.transform.forward = Vector3.forward;
        gun.transform.localScale = Vector3.one;

        gun.rangeMult =
            gun.rateMult =
            gun.damageMult = .5f;

        gun.Initialize(PlayerCharacter.instance);
        gun.ToggleShooting(true);

        angleStep = 360 / guns.Count;
    }

    public void Remove()
    {
        Gun droppedGun = guns.GetRandom();

        droppedGun.transform.SetParent(null);

        droppedGun.ToggleShooting(false);

        guns.Remove(droppedGun);

        droppedGun.rangeMult =
            droppedGun.rateMult =
            droppedGun.damageMult = 1f;
    }

    float T = 0;

    void FixedUpdate()
    {
        T += Time.fixedDeltaTime * rotationAnglePerSecond;
        T %= 360;

        float t = T;

        foreach (var item in guns)
        {
            float x = radius * Mathf.Cos(t * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(t * Mathf.Deg2Rad);

            Vector3 pos = transform.position + new Vector3(x, 0, y);

            item.transform.position = pos;

            t += angleStep;
        }
    }
}
