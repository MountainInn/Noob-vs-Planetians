using HyperCasual.Runner;
using UnityEngine;
using YG;
using Zenject;

public class Vault : MonoBehaviour
{
    static public Vault instance => _inst;
    static Vault _inst;
    Vault(){ _inst = this; }

    [SerializeField] public Currency money;

    [HideInInspector] public int buffer;

    [Inject] void Construct(YandexSaveSystem sv)
    {
        sv.Register(
            save => {
                save.money = money.react.Value;
            },
            load => {
                money.react.Value = load.money;
            });
    }

    public void GainMoney(int amount)
    {
        money.react.Value += amount;
        buffer += amount;
    }

    public void Multiply(int mult)
    {
        buffer *= mult;
    }

    public void Claim()
    {
        money.react.Value += buffer;
        buffer = 0;
    }
}
