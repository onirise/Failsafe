using UnityEngine;
using Zenject;

public class GameplaySavesInstaller : MonoInstaller
{
    [SerializeField]
    GameplaySavesHandler gameplaySavesHandlerInstance;

    public override void InstallBindings()
    {
        
        Container.Bind<GameplaySavesHandler>().FromInstance(gameplaySavesHandlerInstance).AsSingle().NonLazy();
        
    }
}