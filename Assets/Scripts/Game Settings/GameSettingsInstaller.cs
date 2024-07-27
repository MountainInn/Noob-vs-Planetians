using Zenject;
using UnityEngine;

public class GameSettingsInstaller : MonoInstaller
{
    [SerializeField] GameSettings gameSettingsInstance;
    
    public override void InstallBindings()
    {
        Container 
            .Bind<GameSettings>() 
            .FromInstance(gameSettingsInstance)
            .AsCached();
    }
}
