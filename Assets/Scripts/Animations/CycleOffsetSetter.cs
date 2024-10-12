using UnityEngine;

public class CycleOffsetSetter : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] public float cycleOffset;
    [SerializeField] public bool randomOffset;

    void Awake()
    {
        if (randomOffset)
            cycleOffset = Random.value;

        animator.SetFloat("cycleOffset", cycleOffset);
    }
}
