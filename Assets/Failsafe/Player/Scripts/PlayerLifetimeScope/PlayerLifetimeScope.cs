using Failsafe.PlayerMovements;
using Failsafe.Scripts.Health;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.Player
{
    /// <summary>
    /// Регистрация компонентов игрового персонажа
    /// <para/>Дочерний скоуп к <see cref="Failsafe.GameSceneServices.GameSceneLifetimeScope"/>
    /// </summary>
    public class PlayerLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerModelParameters playerParameters;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SimpleHealth>(Lifetime.Singleton).As<IHealth>().WithParameter(playerParameters.MaxHealth);
            //TODO: зарегистрировать компоненты и системы игрока : InputHandler, MovementController, Inventory ...
            
        }
    }
}
