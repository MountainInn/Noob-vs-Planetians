using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MasterPS : MonoBehaviour
{
    ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Fire(Vector3 position)
    {
        ParticleSystem.EmitParams emitParams = new ()
        {
            randomSeed = (uint)(UnityEngine.Random.value * 10000),
            applyShapeToPosition = true,
            position = position,
            rotation3D = new Vector3(0, 0, 0),
        };
    }
}
