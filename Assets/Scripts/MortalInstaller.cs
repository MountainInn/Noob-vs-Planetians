using Zenject;

public class MortalInstaller : MonoInstaller
{
    override public void InstallBindings()
    {
        Container
            .Bind(typeof(Health),
                  typeof(Damage),
                  typeof(Healing)
            )
            .FromComponentSibling();
    }
}
