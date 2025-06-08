using Failsafe.Scripts.Bootstrap;
using Failsafe.Scripts.Configs;
using Failsafe.Scripts.Health;
using UnityEngine;
using UnityEngine.LightTransport;
using VContainer;
using VContainer.Unity;

namespace Failsafe.Scripts.DependencyInjection
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private GameConfig _gameConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameConfig).As<GameConfig>();
            
            builder.Register<SceneLoader.SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

            builder.RegisterEntryPoint<Bootstrapper>();
            
        }
    }
}