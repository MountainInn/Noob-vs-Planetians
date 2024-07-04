using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] int amount;
    [Space]
    [SerializeField] Transform particleOrigin;


    public void __AddToVault( ) => AddToVault();
    public void AddToVault()
    {
        MoneyCache.instance.Add(amount);

        MoneyPS.instance.Fire(particleOrigin.position, 7);
    }
}
