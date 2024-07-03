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

    int lastWeaponIndex = -1;

    [HideInInspector] public Volume expVolume = new();

    StackedNumber expirience = new(0);

    public (Sprite, Sprite) GetWeaponSprites()
    {
        return (fields[lastWeaponIndex].icon,
                fields.ElementAtOrDefault(lastWeaponIndex+1).icon);
    }
   
    void Awake()
    {
        upgradeExpirience.Inject(expirience, l => l);
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
        expVolume.current.Value = expirience.AsFloorInt();

        int cacheLastWeaponIndex = lastWeaponIndex;

        lastWeaponIndex =
            fields
            .FindLastIndex(f => f.expirience <= expirience.AsFloorInt());

        if (lastWeaponIndex == -1)
            lastWeaponIndex = 0;

        if (lastWeaponIndex != cacheLastWeaponIndex)
        {
            expVolume.Resize(fields[lastWeaponIndex].expirience);

            MaybeSwitchWeapon(PlayerCharacter.instance.gunSlot);
        }
    }

    void MaybeSwitchWeapon(GunSlot gunSlot)
    {
        gunSlot.MaybeSwitchEquipment(lastWeaponIndex);
    }

    void Save()
    {
        YandexGame.savesData.weaponExpirience = expirience.AsFloorInt();
        YandexGame.savesData.lastWeaponIndex = lastWeaponIndex;

        YandexGame.SaveProgress();
    }

    void Load()
    {
        upgradeExpirience.level.ware.SetLevel(YandexGame.savesData.weaponExpirience);

        lastWeaponIndex = YandexGame.savesData.lastWeaponIndex;

        OnLevelUp(expirience.AsFloorInt());
    }


    [System.Serializable]
    public struct Field
    {
        [SerializeField] [Min(1)] public int expirience;
        [SerializeField] public Gun gun;
        [SerializeField] public Sprite icon;

    }
}
