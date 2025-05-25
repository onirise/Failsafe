using Failsafe.Scripts;
using Failsafe.Scripts.Bootstrap;
using Failsafe.Scripts.Configs;
using Failsafe.Scripts.SceneLoader;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoInstaller
{
    public GameConfig GameConfig;
    public ISceneLoader SceneLoader;
    public override void InstallBindings()
    {
        Container.Bind<GameConfig>().FromScriptableObject(GameConfig).AsSingle();
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<Bootstrapper>().AsSingle().NonLazy();


    }
}