using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] public int amount;

    public void AddGun()
    {
        MoneyCache.instance.TotalCoins += amount;

        CoinContainer.instance.OnCoinCollected(this);
    }
}
