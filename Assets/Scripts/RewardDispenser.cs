using System;
using UnityEngine;

public class RewardDispenser : MonoBehaviour
{
    static public RewardDispenser instance => _inst ??= FindObjectOfType<RewardDispenser>();
    static RewardDispenser _inst;

    const int MONEY_MULTIPLIER = 0;

    public event Action onClaimX5;

    int multiplier;

    void Awake()
    {
        // YandexGame.RewardVideoEvent += Dispense;
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
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
