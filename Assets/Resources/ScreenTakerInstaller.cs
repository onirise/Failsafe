using UnityEngine;
using Zenject;

public class ScreenTakerInstaller : MonoInstaller
{
    [SerializeField]
    ScreenTaker screenTakerInstance;
    public override void InstallBindings()
    {
        Container.Bind<ScreenTaker>().FromInstance(screenTakerInstance).AsSingle().NonLazy();
    }
}