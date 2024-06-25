using UnityEngine;
using HyperCasual.Runner;

[RequireComponent(typeof(Mortal))]
public class PlayerCharacter : MonoBehaviour, Mortal.IMortalCallback
{
    Mortal mortal;

    void Start()
    {
        mortal = GetComponent<Mortal>();

        var health = mortal.health;
    }

    public void OnHeal(Healing healing)
    {

    }

    public void OnSuffer(Harm harm)
    {
    }

    public void OnDie()
    {
        GameManager.Instance.Lose();
    }
}
