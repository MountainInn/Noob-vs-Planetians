using UnityEngine;

public class DataManager : MonoBehaviour
{
    // public static string Language => YandexGame.EnvironmentData.language;

    [SerializeField] bool clearSave;

    static public int Coins
    {
        get => PlayerPrefs.GetInt("COINS_ID", 0);
        set => PlayerPrefs.SetInt("COINS_ID", value);
    }

    static public int SelectedWeaponIndex
    {
        get => PlayerPrefs.GetInt("WEAPON_ID", 0);
        set => PlayerPrefs.SetInt("WEAPON_ID", value);
    }

    static public int LevelCount
    {
        get => PlayerPrefs.GetInt("LEVEL_COUNT", 0);
        set => PlayerPrefs.SetInt("LEVEL_COUNT", value);
    }

    static public int SimpleLevelIndex
    {
        get => PlayerPrefs.GetInt("CURRENT_LEVEL_ID", 0);
        set => PlayerPrefs.SetInt("CURRENT_LEVEL_ID", value);
    }

    void Awake()
    {
        if (clearSave)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
