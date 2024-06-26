using HyperCasual.Runner;
using UnityEngine;

public class MoneyCache : MonoBehaviour
{
    static public MoneyCache instance => _inst ??= FindObjectOfType<MoneyCache>();
    static MoneyCache _inst;

    [HideInInspector] public int coinsOnThisLevel;

    public int TotalCoins
    {
        get => DataManager.Coins;
        set => DataManager.Coins = value;
    }

    void Start()
    {
        GameManager.Instance.onStartGame.AddListener(Clear);
    }

    public void AddCoins(int obj)
    {
        coinsOnThisLevel += obj;
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
}
