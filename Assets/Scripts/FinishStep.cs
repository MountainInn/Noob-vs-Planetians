using UnityEngine;
using TMPro;
using System;

public class FinishStep : MonoBehaviour
{
    static public Action<int> onStepped;

    [SerializeField] [Min(1)] int mult;
    [Space]
    [SerializeField] TextMeshPro label;

    void OnValidate()
    {
        mult = transform.GetSiblingIndex() + 1;

        label.text = $"x{mult}";
    }

    public void __InvokeOnStepped( ) => InvokeOnStepped();
    public void InvokeOnStepped()
    {
        onStepped?.Invoke(mult);
    }
}
