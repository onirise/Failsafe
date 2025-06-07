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
        [SerializeField] PlayerHealthUI _playerHealthUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SimpleHealth>(Lifetime.Singleton).As<IHealth>();
            
            builder.RegisterInstance(_gameConfig).As<GameConfig>();
            
            builder.Register<SceneLoader.SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();
            
            builder.RegisterComponentInHierarchy<PlayerHealthUI>();

            builder.RegisterEntryPoint<Bootstrapper>();
            
        }
    }
}