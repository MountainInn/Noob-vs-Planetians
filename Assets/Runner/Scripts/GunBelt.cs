using System.Collections.Generic;
using UnityEngine;

public class GunBelt : MonoBehaviour
{
    static public GunBelt instance => _inst ??= FindObjectOfType<GunBelt>();
    static GunBelt _inst;

    [SerializeField] [Min(1)] float radius;
    [SerializeField] float rotationAnglePerSecond = 60;

    List<Gun> guns = new();

    float angleStep = 0;

    public void Add(Gun gun)
    {
        guns.Add(gun);

        gun.ToggleShooting(true);

        angleStep = 360 / guns.Count;
    }

    public void Remove()
    {
        Gun droppedGun = guns.GetRandom();

        droppedGun.ToggleShooting(false);

        guns.Remove(droppedGun);
    }

    float T = 0;

    void FixedUpdate()
    {
        T += Time.fixedDeltaTime * rotationAnglePerSecond;
        T %= 360;

        float t = T;

        foreach (var item in guns)
        {
            float x = radius * Mathf.Cos(t);
            float y = radius * Mathf.Sin(t);

            Vector3 pos = transform.position + new Vector3(x, 0, y);

            item.transform.position = pos;

            t += angleStep;
        }
    }
}
