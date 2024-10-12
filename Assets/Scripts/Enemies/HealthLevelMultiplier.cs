using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthLevelMultiplier : MonoBehaviour
{
    [SerializeField] float multPerLevel = .25f;

    void Awake()
    {
        GetComponent<Health>()
            .Value
            .SetMultiplier("Level Mult", 1 + Flow.instance.LevelCount * multPerLevel);
    }
}
