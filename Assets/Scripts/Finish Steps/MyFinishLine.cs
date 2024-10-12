using UnityEngine;
using HyperCasual.Runner;

[RequireComponent(typeof(InteractiveCollider))]
public class MyFinishLine : MonoBehaviour
{
    public void GameOver()
    {
        GameManager.Instance.Lose();
    }
}
