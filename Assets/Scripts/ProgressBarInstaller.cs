using UnityEngine;
using Zenject;

public class ProgressBarInstaller : MonoInstaller
{
    [SerializeField] ProgressBar healthBar;

    override public void InstallBindings()
    {
        Container
            .BindInstance(healthBar)
            .WhenInjectedInto<PlayerCharacter>();
    }
}
