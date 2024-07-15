using UnityEngine;

public class CycleOffsetSetter : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] public float cycleOffset;

    void Awake()
    {
        animator.SetFloat("cycleOffset", cycleOffset);
    }
}
