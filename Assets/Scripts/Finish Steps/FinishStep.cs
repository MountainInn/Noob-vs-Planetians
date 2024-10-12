using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class FinishStep : MonoBehaviour
{
    static public Action<int> onStepped;

    [SerializeField] [Min(1)] public int mult;
    [Space]
    [SerializeField] [Min(0)] float tweenDuration;
    [Space]
    [SerializeField] TextMeshPro label;
    [SerializeField] public MeshRenderer stepMeshRenderer;
    [SerializeField] public MeshFilter stepMeshFilter;

    MeshRenderer meshRenderer;

    void OnValidate()
    {
        if (label)
            label.text = $"x{mult}";
    }

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void __InvokeOnStepped( ) => InvokeOnStepped();
    public void InvokeOnStepped()
    {
        onStepped?.Invoke(mult);

        meshRenderer.material
            .DOFloat(.5f, "_Projectors_Angle", tweenDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.OutCubic);
    }
}
