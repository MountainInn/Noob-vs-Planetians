using Zenject;

public class MortalInstaller : MonoInstaller
{
    override public void InstallBindings()
    {
        Container
            .Bind(typeof(Mortal),
                  typeof(Harm),
                  typeof(Healing)
            )
            .FromComponentSibling();
    }
}
