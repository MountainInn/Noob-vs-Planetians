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

    [SerializeField] public Volume levelVolume = new(6);
    [SerializeField] public Volume expirienceVolume = new(5);
    [Space]
    [SerializeField] List<Field> fields;
    [Space]
    [SerializeField] public UnityEvent onNewWeaponUnlocked;

    int currentWeaponIndex => (int)levelVolume.current.Value;
    int currentExpirience => (int)expirienceVolume.current.Value;

    public (Sprite, Sprite) GetWeaponSprites()
    {
        return (fields[currentWeaponIndex].icon,
                fields.ElementAtOrDefault(currentWeaponIndex+1).icon);
    }

    void OnValidate()
    {
        fields.ResizeDestructive((int)levelVolume.maximum.Value);
    }
   
    void Awake()
    {
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    public void AddExpirience(int amount)
    {
        if (expirienceVolume.Add(amount))
        {
            if (levelVolume.Add(1))
            {

            }
            OnLevelUp();
        }
    }

    void OnLevelUp()
    {
        expirienceVolume.Resize(fields[currentWeaponIndex].expirience);

        MaybeSwitchWeapon(PlayerCharacter.instance.gunSlot);
    }

    void MaybeSwitchWeapon(GunSlot gunSlot)
    {
        gunSlot.MaybeSwitchEquipment(currentWeaponIndex);
    }

    void Save()
    {
        YandexGame.savesData.weaponExpirience = currentExpirience;
        YandexGame.savesData.lastWeaponIndex = currentWeaponIndex;

        YandexGame.SaveProgress();
    }

    void Load()
    {
        expirienceVolume.current.Value = YandexGame.savesData.weaponExpirience;
        levelVolume.current.Value = YandexGame.savesData.lastWeaponIndex;

        OnLevelUp();
    }


    [System.Serializable]
    public struct Field
    {
        [SerializeField] [Min(1)] public int expirience;
        [SerializeField] public Sprite icon;

    }
}
