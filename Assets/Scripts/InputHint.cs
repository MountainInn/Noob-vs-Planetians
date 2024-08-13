using UnityEngine;
using Zenject;

public class InputHint : MonoBehaviour
{
    bool isFirstSession;
    
    [Inject] void Construct(YandexSaveSystem sv)
    {
        sv.Register(
            save => {
                
                },
            load => {
                isFirstSession = load.isFirstSession;
            });
    }

    void Start()
    {
        if (!isFirstSession)
            gameObject.SetActive(false);
    }
}
