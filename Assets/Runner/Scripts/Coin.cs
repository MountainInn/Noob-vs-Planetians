using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] public int amount;

    public void Collect()
    {
        DataManager.Coins += amount;

        CoinContainer.instance.OnCoinCollected(this);
    }
}
