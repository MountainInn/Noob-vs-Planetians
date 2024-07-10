using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class WeaponExpirienceView : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;
    [SerializeField] Image currentWeaponImage;
    [SerializeField] Image nextWeaponImage;

    void Awake()
    {
        progressBar
            .Subscribe(WeaponExperience.instance.gameObject,
                       WeaponExperience.instance.expirienceVolume)
            .AddTo(this);

        UpdateWeaponSprites();

        WeaponExperience.instance
            .onNewWeaponUnlocked.AddListener(() =>
            {
                UpdateWeaponSprites();
            });
    }

    void UpdateWeaponSprites()
    {
        (currentWeaponImage.sprite,
         nextWeaponImage.sprite) = WeaponExperience.instance.GetWeaponSprites();
    }
}
