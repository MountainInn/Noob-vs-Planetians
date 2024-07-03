using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Adometer : MonoBehaviour
{
    static public Adometer instance => _inst ??= FindObjectOfType<Adometer>();
    static Adometer _inst;

    [SerializeField] [Range(-180, 0)] int startingAngle;
    [SerializeField] [Range(0, 180)] int endingAngle;
    [SerializeField] [Min(30)] int rotationSpeed;
    [Space]
    [SerializeField] [Min(1)] int numberOfZones = 5;
    [Space]
    [SerializeField] Image scale;
    [SerializeField] Image arrow;
    [SerializeField] TextMeshProUGUI labelCurrentMultiplier;

    int scaleSize;
    int zoneSize;
    int arrowAngle;
    int resultZone;

    Tween arrowRotation;

    public int StopArrow()
    {
        arrowRotation?.Kill();

        return resultZone;
    }

    int GetCurrentMultiplier()
    {
        scaleSize = endingAngle - startingAngle;

        zoneSize = scaleSize / numberOfZones;

        arrowAngle = (int)arrow.transform.localRotation.eulerAngles.z - startingAngle;

        resultZone = arrowAngle / zoneSize;

        return resultZone;
    }

    void OnEnable()
    {
        arrow.transform.localRotation = Quaternion.Euler(0, 0, startingAngle);

        arrowRotation =
            arrow
            .transform
            .DOLocalRotate(new Vector3(0, 0, endingAngle), rotationSpeed)
            .SetSpeedBased(true)
            .SetLoops(-1, LoopType.Yoyo)
            .OnUpdate(() => labelCurrentMultiplier.text = $"Get x{GetCurrentMultiplier()}!");
    }

    void OnDisable()
    {
        StopArrow();
    }
}
