using Failsafe.GameSceneServices.SpawnSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.GameSceneServices
{
    /// <summary>
    /// Регистрация сервисов и компонентов игровой сцены, общих для объектов на сцене или не привязаных к конкретному объекту
    /// <para/>Дочерний скоуп к <see cref="Failsafe.Scripts.DependencyInjection.RootLifetimeScope"/>
    /// </summary>
    public class GameSceneLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private EnemySpawnSystemBuilder _enemySpawnSystemBuilder;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_enemySpawnSystemBuilder);
            //TODO: зарегистрировать системы игровой сцены : SignalManager, SpawnSystem ...

            //TODO: Пока это монобэх, нужно интегрировать врагов с VContainer
            builder.RegisterComponentInHierarchy<SignalManager>();
            
            builder.RegisterEntryPoint<EnemySpawnSystem>().AsSelf();
        }
    }
}