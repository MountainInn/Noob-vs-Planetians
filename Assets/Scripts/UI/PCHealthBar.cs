using UnityEngine;

public class PCHealthBar : MonoBehaviour
{
    static public PCHealthBar instance => _inst ??= FindObjectOfType<PCHealthBar>();
    static PCHealthBar _inst;

    [SerializeField] ProgressBar prog;

    System.IDisposable subscription;

    public void Resubscribe()
    {
        subscription?.Dispose();
        subscription =
            prog.Subscribe(PlayerCharacter.instance.gameObject,
                           PlayerCharacter.instance.health.Volume);
    }
}
