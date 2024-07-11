using UnityEngine;

public class FinishStepsParent : MonoBehaviour
{
    [SerializeField] Material stepMaterial;
    [SerializeField] Mesh[] stepMeshes;
    [Space]
    [SerializeField] Mesh[] ufoMeshes;
    [SerializeField] [Min(1)] int ufoMeshesStrip;
    [SerializeField] [Min(1)] int healthPerStep;
    [Space]
    [SerializeField] [Min(1)] int stepLength = 5;

    int CalculateHealthOnStep(int step)
    {
        return healthPerStep * step;
    }

    [ContextMenu("Validate")]
    public void OnValidate()
    {
        foreach (var (i, step) in GetComponentsInChildren<FinishStep>().Enumerate())
        {
            step.stepMeshRenderer.material = stepMaterial;
            step.stepMeshFilter.mesh = stepMeshes[i % stepMeshes.Length];

            step.transform.localPosition = step.transform.localPosition.WithZ(i * stepLength);

            step.mult = i + 1;

            foreach (var (j, ufo) in step.GetComponentsInChildren<UFOMaterialScroller>().Enumerate())
            {
                ufo.offsetX = (i + j) % 10;
                ufo.offsetY = (i + j) % 2;

                ufo.mesh = ufoMeshes[i / ufoMeshesStrip % ufoMeshes.Length];

                ufo.OnValidate();

                ufo
                    .GetComponent<Health>()
                    .Value
                    .SetInitial(CalculateHealthOnStep(i+1));
            }
        }
    }
}
