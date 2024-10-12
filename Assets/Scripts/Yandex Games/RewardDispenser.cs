using System;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class RewardDispenser : MonoBehaviour
{
    static public RewardDispenser instance => _inst ??= FindObjectOfType<RewardDispenser>();
    static RewardDispenser _inst;

    [SerializeField] public UnityEvent onClaimX5;
    [SerializeField] public UnityEvent onResurrect;

    const int MONEY_MULTIPLIER = 0;
    const int FREE_UPGRADE = 1;
    const int RESURRECT = 2;

    int multiplier;

    void Awake()
    {
        YandexGame.RewardVideoEvent += Dispense;
    }

    Upgrade freeUpgrade;

    public void ShowFreeUpgrade(Upgrade upgrade)
    {
        this.freeUpgrade = upgrade;

        YandexGame.RewVideoShow(FREE_UPGRADE);
    }

    public void ShowMoneyMult(int multiplier)
    {
        this.multiplier = multiplier;

        YandexGame.RewVideoShow(MONEY_MULTIPLIER);
    }

    public void ShowResurrect()
    {
        YandexGame.RewVideoShow(RESURRECT);
    }

    void Dispense(int rewardId)
    {
        switch (rewardId)
        {
            case MONEY_MULTIPLIER:

                Vault.instance.Multiply(multiplier);

                onClaimX5.Invoke();

                break;

            case FREE_UPGRADE:

                OnFreeUpgrade();

                break;

            case RESURRECT:

                onResurrect.Invoke();

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
