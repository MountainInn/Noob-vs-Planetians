using HyperCasual.Runner;
using UnityEngine;
using YG;

public class Vault : MonoBehaviour
{
    static public Vault instance => _inst;
    static Vault _inst;
    Vault(){ _inst = this; }

    [SerializeField] public Currency money;

    [HideInInspector] public int buffer;

    void OnEnable()
    {
        YandexGame.GetDataEvent += Load;
    }
    void OnDisable()
    {
        YandexGame.GetDataEvent -= Load;
    }

    void Start()
    {
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


    void Save()
    {
        YandexGame.savesData.money = money.react.Value;
        YandexGame.SaveProgress();
    }
    void Load()
    {
        money.react.Value = YandexGame.savesData.money;
    }
}
