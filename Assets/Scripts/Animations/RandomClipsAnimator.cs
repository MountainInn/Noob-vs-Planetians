using UnityEngine;
using System.Collections.Generic;

public class RandomClipsAnimator : MonoBehaviour
{
    [SerializeField] AnimationClip[] idleClips;
    [Space]
    [SerializeField] Animator animator;

    AnimationClip
        idle;


    void Awake()
    {
        idle = idleClips.GetRandom();

        AnimatorOverrideController animatorOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        foreach (var a in animatorOverride.animationClips)
        {
            var anim = a.name switch
                {
                    "Offensive Idle" => idle,
                    _ => throw new System.ArgumentException()
                };

            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, anim));
        }

        animatorOverride.ApplyOverrides(anims);

        animator.runtimeAnimatorController = animatorOverride;
    }
}
