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
        healing.Value = (int)(PlayerCharacter.instance.health.Value.AsFloat() * healthPercent);

        UpdateLabel();
    }

    void UpdateLabel()
    {
        label.text = healthPercent.ToString("P0");
    }
}
