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
    [Space]
    [SerializeField] [Range(0, 1f)] float cycleOffsetPerStep = .1f;

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
                ufo.offsetX = (i) % 10;
                ufo.offsetY = UnityEngine.Random.Range(0, 2);

                ufo.mesh = ufoMeshes[i / ufoMeshesStrip % ufoMeshes.Length];

                ufo.OnValidate();

                var healthMults = ufo.GetComponent<Health>().Value.serializedMultipliers;

                float stepMult = 1 + Mathf.Pow(i, 1.1f);

                if (healthMults.Count == 0)
                {
                    healthMults.Add(stepMult);
                }
                else
                    healthMults[0] = stepMult;


                ufo
                    .GetComponentInChildren<CycleOffsetSetter>()
                    .cycleOffset = i * cycleOffsetPerStep;

                ufo
                    .GetComponent<Health>()
                    .Value
                    .SetInitial(CalculateHealthOnStep(i+1));
            }
        }
    }
}
