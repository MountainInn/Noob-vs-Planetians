using DG.Tweening;
using UnityEngine;
using TMPro;
using HyperCasual.Runner;

public class CoinContainer : MonoBehaviour
{
    static public CoinContainer instance => _inst ??= FindObjectOfType<CoinContainer>();
    static CoinContainer _inst;
   
    [SerializeField] Transform coinTargetPoint;
    [SerializeField] TextMeshProUGUI label;

    Tween punchTween;

    public void OnCoinCollected(Coin coin)
    {
        // coin.transform
        //     .DOMove(coinTargetPoint.position,
        //             1)
        //     .OnKill(() =>
        //     {
        int prevCoins = MoneyCache.instance.TotalCoins;
        int newCoins = prevCoins + coin.amount;

        DoTweenSelf(prevCoins, newCoins);

        GameObject.Destroy(coin.gameObject);
        // });
    }

    void DoTweenSelf(int prevCoins, int newCoins)
    {
        float duration = .75f;

        DOVirtual.Int(prevCoins, newCoins, duration,
                      (val) => {
                          label.text = $"{val}";
                      }
        );
        // .OnComplete(() => GameState.Instance.LoadNextLevel());

        if (punchTween == null)
            punchTween =
                transform
                .DOPunchScale(transform.lossyScale * 1.05f, duration / 4)
                .OnKill(() => punchTween = null);
    }

}
