using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class WeaponExpirienceView : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;
    [SerializeField] Image currentWeaponImage;
    [SerializeField] Image nextWeaponImage;
    [SerializeField] TextMeshProUGUI labelMax;

    IDisposable progressBarSub;
    
    void Awake()
    {
        progressBarSub =
            progressBar
            .Subscribe(WeaponExperience.instance.gameObject,
                       WeaponExperience.instance.expirienceVolume);
        
        labelMax.enabled = false;
    }

    void Start()
    {
        UpdateWeaponSprites();

        WeaponExperience.instance
            .level.onSetLevel.AddListener((_) =>
            {
                UpdateWeaponSprites();

                if (nextWeaponImage.sprite == null)
                {
                    nextWeaponImage.enabled = false;

                    progressBarSub?.Dispose();
                    progressBarSub = null;
                    
                    labelMax.enabled = true;

                    progressBar.SetFull();
                }
            });
    }

    void UpdateWeaponSprites()
    {
        (currentWeaponImage.sprite,
         nextWeaponImage.sprite) = WeaponExperience.instance.GetWeaponSprites();
    }
}
