using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class Adometer : MonoBehaviour
{
    static public Adometer instance => _inst;
    static Adometer _inst;
    Adometer(){ _inst = this; }

    [SerializeField] int[] multipliers;
    [Space]
    [SerializeField] [Min(30)] int scrollSpeed;
    [Space]
    [SerializeField] RectTransform zonesParent;
    [SerializeField] RectTransform arrowParent;
    [SerializeField] Image arrow;
    [Space]
    [SerializeField] TextMeshProUGUI labelCurrentMultiplier;

    RectTransform[] zones;

    Tween arrowScrollTween;

    void OnValidate()
    {
        foreach (var (label, mult) in
                 GetComponentsInChildren<TextMeshProUGUI>()
                 .Zip(multipliers))
        {
            label.text = $"x{mult}";
        }
    }

    void Awake()
    {
        zones =
            zonesParent
            .GetComponentsInChildren<RectTransform>()
            .Where(rt => (rt.parent == zonesParent.transform))
            .ToArray();
    }

    public void StartArrow()
    {
        arrow.transform.localPosition = Vector3.zero;

        arrowScrollTween =
            arrow
            .transform
            .DOLocalMoveX(arrowParent.sizeDelta.x, scrollSpeed)
            .SetSpeedBased(true)
            .SetLoops(-1, LoopType.Yoyo)
            .OnUpdate(() => labelCurrentMultiplier.text = $"Get x{GetCurrentMultiplier()}!");
    }

    public int StopArrow()
    {
        arrowScrollTween?.Kill();

        return GetCurrentMultiplier();
    }

    int GetCurrentMultiplier()
    {
        int zoneIndex =
            zones
            .Enumerate()
            .First(tup =>
                   RectTransformUtility
                   .RectangleContainsScreenPoint(tup.value, arrow.rectTransform.position))
            .index;

        return multipliers[zoneIndex];
    }
}
