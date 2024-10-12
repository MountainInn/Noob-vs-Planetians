using UnityEngine;
using DG.Tweening;

public class TweenElasticScale : MonoBehaviour
{
    [SerializeField] float delay = 1f;
    [SerializeField] float duration = 1f;
    [SerializeField] float amplitude = .1f;

    void Start()
    {
        transform
            .DOScale(Vector3.one * (1+amplitude), duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(delay)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
