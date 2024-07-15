using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] int amount;
    [Space]
    [SerializeField] Transform particleOrigin;


    public void __SetAmount(int amount) => SetAmount(amount);
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public void __AddToVault( ) => AddToVault();
    public void AddToVault()
    {
        Vault.instance.GainMoney(amount);

        MoneyPS.instance.Fire(particleOrigin.position, 7);
    }
}
