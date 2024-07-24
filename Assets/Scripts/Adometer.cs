using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class Adometer : MonoBehaviour
{
    static public Adometer instance => _inst;
    static Adometer _inst;
    Adometer() { _inst = this; }

    [SerializeField] Canvas rootCanvas;
    [SerializeField] int[] multipliers;
    [Space]
    [SerializeField][Min(30)] int scrollSpeed;
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
        arrow.rectTransform.anchoredPosition = Vector3.zero;

        arrowScrollTween =
            arrow
            .rectTransform
            .DOAnchorPosX(arrowParent.rect.width, scrollSpeed)
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
        Vector2 canvasScale = rootCanvas.transform.localScale.xy();

        var arrowRect = GetWorldRect(arrow.rectTransform, canvasScale);

        Vector2 arrowPoint = arrowRect.center + new Vector2(0, arrowRect.height / 2);

        int zoneIndex =
            zones
            .Enumerate()
            .First(tup => GetWorldRect(tup.value, canvasScale).Contains(arrowPoint))
            .index;

        return multipliers[zoneIndex];

        static Rect GetWorldRect(RectTransform rt, Vector2 scale)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            Vector3 topLeft = corners[0];

            Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x,
                                             scale.y * rt.rect.size.y);

            return new Rect(topLeft, scaledSize);
        }
    }
}
