using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class TabletInstaller : MonoInstaller
{
    [SerializeField]
    TabletHandler tabletHandlerInstance;

    public override void InstallBindings()
    {
               
        Container.Bind<TabletHandler>().FromInstance(tabletHandlerInstance).AsSingle().NonLazy();
    }
}