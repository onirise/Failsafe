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
        protected override void Configure(IContainerBuilder builder)
        {
            //TODO: зарегистрировать системы игровой сцены : SignalManager, SpawnSystem ...
        }
    }
}