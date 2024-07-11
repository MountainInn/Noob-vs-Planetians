using System;
using UnityEngine;
using System.Linq;

public class UFOMaterialScroller : MonoBehaviour
{
    [SerializeField] [Range(0, 9)] public int offsetX;
    [SerializeField] [Range(0, 1)] public int offsetY;
    [Space]
    [SerializeField] public Mesh mesh;
    [Space]
    [SerializeField] Vector2 offsetStep = new Vector2(.1f, .5f);
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshFilter meshFilter;

    public void OnValidate()
    {
        if (meshRenderer)
        {
            meshRenderer.sharedMaterial.mainTextureOffset = CalculateOffset();
        }

        if (meshFilter)
        {
            meshFilter.mesh = mesh;
        }
    }

    void Start()
    {
        meshRenderer.material.mainTextureOffset = CalculateOffset();
        meshFilter.mesh = mesh;
    }

    Vector2 CalculateOffset()
    {
        var offset = new Vector2(offsetX, offsetY);
        offset.Scale(offsetStep);
        return offset;
    }
}
