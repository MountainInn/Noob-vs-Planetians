using Zenject;

public class YandexSaveSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<YandexSaveSystem>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();
    }
}
