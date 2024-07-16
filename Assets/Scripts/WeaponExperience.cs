using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using YG;
using UnityEngine.Events;
using Zenject;

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

    [Inject] void SubToSaveSystem(YandexSaveSystem sv)
    {
        sv.Register(
            save => {
                save.lastWeaponIndex = level.L;
                save.weaponExpirience = currentExpirience;
            },
            load => {
                level.SetLevel(load.lastWeaponIndex);
                expirienceVolume.current.Value = load.weaponExpirience;
            });
    }

    public int currentLevel => level.L;
    public int currentExpirience => (int)expirienceVolume.current.Value;


    void OnValidate()
    {
        fields.ResizeDestructive((int)level.Volume.maximum.Value + 1);
    }

    void Awake()
    {
        level.SetLevel(level.L);
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

    [System.Serializable]
    public class Field
    {
        [SerializeField] [Min(0)] public int expirience;
        [SerializeField] public Sprite icon;

    }
}
