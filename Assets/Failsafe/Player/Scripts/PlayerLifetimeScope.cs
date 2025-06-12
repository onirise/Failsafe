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
        protected override void Configure(IContainerBuilder builder)
        {
            //TODO: зарегистрировать компоненты и системы игрока : InputHandler, MovementController, Inventory ...
        }
    }
}
