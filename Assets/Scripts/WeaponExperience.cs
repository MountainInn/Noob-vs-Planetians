using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using YG;
using UnityEngine.Events;

public class WeaponExperience : MonoBehaviour
{
    static public WeaponExperience instance => _inst ??= FindObjectOfType<WeaponExperience>();
    static WeaponExperience _inst;

    [SerializeField] public Upgrade upgradeExpirience;
    [Space]
    [SerializeField] List<Field> fields;
    [Space]
    [SerializeField] public UnityEvent onNewWeaponUnlocked;

    int exp,
        lastWeaponIndex;

    [HideInInspector] public Volume expVolume = new();

    public (Sprite, Sprite) GetWeaponSprites()
    {
        return (fields[lastWeaponIndex].icon,
                fields.ElementAtOrDefault(lastWeaponIndex+1).icon);
    }
   
    void Awake()
    {
        upgradeExpirience.Inject(OnLevelUp);
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    void OnLevelUp(int l)
    {
        exp = l;
        expVolume.current.Value = exp;

        int cacheLastWeaponIndex = lastWeaponIndex;

        lastWeaponIndex =
            fields
            .FindLastIndex(f => f.expirience <= exp);

        if (lastWeaponIndex != cacheLastWeaponIndex)
        {
            expVolume.Resize(fields[lastWeaponIndex].expirience);

            onNewWeaponUnlocked?.Invoke();
        }
    }

    void Save()
    {
        YandexGame.savesData.weaponExpirience = exp;
        YandexGame.savesData.lastWeaponIndex = lastWeaponIndex;

        YandexGame.SaveProgress();
    }

    void Load()
    {
        exp = YandexGame.savesData.weaponExpirience;
        lastWeaponIndex = YandexGame.savesData.lastWeaponIndex;

        OnLevelUp(exp);
    }


    [System.Serializable]
    public struct Field
    {
        [SerializeField] [Min(1)] public int expirience;
        [SerializeField] public Gun gun;
        [SerializeField] public Sprite icon;

    }
}
