using UnityEngine;

public class MoneyPS : MonoBehaviour
{
    static public MoneyPS instance => _inst ??= FindObjectOfType<MoneyPS>();
    static MoneyPS _inst;

    [SerializeField] public ParticleSystem ps;

    public void Fire(Vector3 position)
    {
        ParticleSystem.EmitParams emitParams = new ()
        {
            randomSeed = (uint)(UnityEngine.Random.value * 10000),
            applyShapeToPosition = true,
            position = position,
            rotation3D = new Vector3(-90, 0, 0),
        };
    }
}
