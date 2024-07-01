using UnityEngine;
using UnityEngine.UI;

public class WeaponExpirienceView : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;
    [SerializeField] Image currentWeaponImage;
    [SerializeField] Image nextWeaponImage;

    void Start()
    {
        progressBar
            .SetVolume(WeaponExperience.instance.expVolume);

        WeaponExperience.instance
            .onNewWeaponUnlocked.AddListener(() =>
            {
                (currentWeaponImage.sprite,
                 nextWeaponImage.sprite) = WeaponExperience.instance.GetWeaponSprites();
            });
    }
}
