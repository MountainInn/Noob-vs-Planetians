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
                isFirstSession = (load.levelCount == 0);
            });
    }

    void Start()
    {
        if (!isFirstSession)
            gameObject.SetActive(false);
    }
}
