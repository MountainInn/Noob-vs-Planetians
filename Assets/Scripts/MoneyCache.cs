using HyperCasual.Runner;
using UnityEngine;
using YG;

public class MoneyCache : MonoBehaviour
{
    static public MoneyCache instance => _inst;
    static MoneyCache _inst;
    MoneyCache(){ _inst = this; }

    [HideInInspector] public int coinsOnThisLevel;

    public int TotalCoins;

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }
    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    void Start()
    {
        GameManager.Instance.onStartGame.AddListener(Clear);
    }

    public void Add(int amount)
    {
        coinsOnThisLevel += amount;
    }

    public void Multiply(int mult)
    {
        coinsOnThisLevel *= mult;
    }

    public void Claim()
    {
        TotalCoins += coinsOnThisLevel;

        Clear();
    }

    void ClaimTween()
    {
    }

    void Clear()
    {
        coinsOnThisLevel = 0;
    }

    void Save()
    {
        YandexGame.savesData.totalCoins = TotalCoins;
        YandexGame.SaveProgress();
    }
    void Load()
    {
        TotalCoins = YandexGame.savesData.totalCoins;
    }
}
