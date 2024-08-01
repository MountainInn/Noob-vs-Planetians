using UnityEngine;

public class EnemyDamageInitializer : MonoBehaviour
{
    [SerializeField] Damage damage;
    
    void Awake()
    {
        float damageValue = Mathf.Max(1, Flow.instance.CurrentLevel / 2);
        
        damage.Value.SetInitial(damageValue);
    }
}
