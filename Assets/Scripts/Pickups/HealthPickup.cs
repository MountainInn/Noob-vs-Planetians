using UnityEngine;
using TMPro;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] [Range(0, 1f)] float healthPercent;
    [Space]
    [SerializeField] TextMeshPro label;
    [SerializeField] Healing healing;

    void OnValidate()
    {
        UpdateLabel();
    }

    void Start()
    {
        float playerMaxHealth = UpgradeHold.instance.upgradeHealth.stat.AsFloat();
        
        healing.Value = (int)(playerMaxHealth * healthPercent);

        UpdateLabel();
    }

    void UpdateLabel()
    {
        label.text = healthPercent.ToString("P0");
    }
}
