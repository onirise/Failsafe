using UnityEngine;
using Zenject;

public class TabletInstaller : MonoInstaller
{
    [SerializeField]
    TabletHandler tabletHandlerInstance;

    public override void InstallBindings()
    {
        Container.Install<SaveManagerInst>();
        Container.Bind<TabletHandler>().FromInstance(tabletHandlerInstance).AsSingle().NonLazy();
    }
}