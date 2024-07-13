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

    [SerializeField] public Level level;
    [SerializeField] public Volume expirienceVolume = new(5);
    [Space]
    [SerializeField] List<Field> fields;
    [Space]
    [SerializeField] public UnityEvent onNewWeaponUnlocked;

    public int currentLevel => level.L;
    public int currentExpirience => (int)expirienceVolume.current.Value;


    void OnValidate()
    {
        fields.ResizeDestructive((int)level.Volume.maximum.Value);
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

    public (Sprite, Sprite) GetWeaponSprites()
    {
        return (fields.ElementAtOrDefault(currentLevel  )?.icon,
                fields.ElementAtOrDefault(currentLevel+1)?.icon);
    }

    public void AddExpirience(int amount)
    {
        if (expirienceVolume.Add(amount))
        {
            level.Up();
        }
    }

    public void OnLevelUp(int l)
    {
        expirienceVolume.ResetToZero();
        expirienceVolume.Resize(fields[l].expirience);

        PlayerCharacter.instance.gunSlot
            .MaybeSwitchEquipment(l);
    }

    void Save()
    {
        YandexGame.savesData.lastWeaponIndex = level.L;
        YandexGame.savesData.weaponExpirience = currentExpirience;

        YandexGame.SaveProgress();
    }

    void Load()
    {
        level.SetLevel(YandexGame.savesData.lastWeaponIndex);
        expirienceVolume.current.Value = YandexGame.savesData.weaponExpirience;
    }

    [System.Serializable]
    public class Field
    {
        [SerializeField] [Min(0)] public int expirience;
        [SerializeField] public Sprite icon;

    }
}
