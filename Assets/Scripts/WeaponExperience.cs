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

    public int currentWeaponIndex => (int)levelVolume.current.Value;
    public int currentExpirience => (int)expirienceVolume.current.Value;

    public (Sprite, Sprite) GetWeaponSprites()
    {
        return (fields[currentWeaponIndex].icon,
                fields.ElementAtOrDefault(currentWeaponIndex+1).icon);
    }

    void OnValidate()
    {
        fields.ResizeDestructive((int)levelVolume.maximum.Value);
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    void OnApplicationQuit()
    {
        Save();
    }

    public void AddExpirience(int amount)
    {
        if (expirienceVolume.Add(amount))
        {
            if (levelVolume.Add(1))
            {
                expirienceVolume.ResetToZero();
            }
            OnLevelUp();
        }
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
        levelVolume.current.Value = YandexGame.savesData.lastWeaponIndex;

        expirienceVolume.Resize(fields[currentWeaponIndex].expirience);

        expirienceVolume.current.Value = YandexGame.savesData.weaponExpirience;

        MaybeSwitchWeapon(PlayerCharacter.instance.gunSlot);
    }

    void OnLevelUp()
    {
        expirienceVolume.Resize(fields[currentWeaponIndex].expirience);

        MaybeSwitchWeapon(PlayerCharacter.instance.gunSlot);
    }


    [System.Serializable]
    public struct Field
    {
        [SerializeField] [Min(1)] public int expirience;
        [SerializeField] public Sprite icon;

    }
}
