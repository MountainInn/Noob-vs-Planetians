using System;
using UnityEngine;

public class RewardDispenser : MonoBehaviour
{
    static public RewardDispenser instance => _inst ??= FindObjectOfType<RewardDispenser>();
    static RewardDispenser _inst;

    const int MONEY_MULTIPLIER = 0;
    const int FREE_UPGRADE = 1;

    public event Action onClaimX5;

    int multiplier;

    void Awake()
    {
        // YandexGame.RewardVideoEvent += Dispense;
    }

    Upgrade freeUpgrade;

    public void ShowFreeUpgrade(Upgrade upgrade)
    {
        this.freeUpgrade = upgrade;
        // YandexGame.RewVideoShow(FREE_UPGRADE);
    }

    public void ShowMoneyX5()
    {
        multiplier = Adometer.instance.StopArrow();

        // YandexGame.RewVideoShow(MONEY_MULTIPLIER);
    }

    void Dispense(int rewardId)
    {
        switch (rewardId)
        {
            case MONEY_MULTIPLIER:

                onClaimX5?.Invoke();

                break;

            case FREE_UPGRADE:

                OnFreeUpgrade();

                break;


            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    void OnFreeUpgrade()
    {
        freeUpgrade.level.Steal();
    }
}
