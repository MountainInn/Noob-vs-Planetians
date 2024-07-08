using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MasterPS : MonoBehaviour
{
    ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Fire(Vector3 position, int amount)
    {
        Fire(position, Vector3.zero, amount);
    }

    public void Fire(Vector3 position, Vector3 rotation, int amount)
    {
        ParticleSystem.EmitParams emitParams = new ()
        {
            randomSeed = (uint)(UnityEngine.Random.value * 10000),
            applyShapeToPosition = true,
            position = position,
            rotation3D = rotation,
        };

        ps.Emit(emitParams, amount);
    }
}
