using Failsafe.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.GameSceneServices
{
    /// <summary>
    /// Регистрация сервисов и компонентов игровой сцены, общих для всей сцены
    /// </summary>
    public class GameSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneDependency>(Lifetime.Scoped);
            //TODO: зарегистрировать системы игровой сцены : SignalManager, SpownSystem ...
        }
    }
    //TODO: Все что ниже используется для теста зависимостей, удалить
    public class SceneDependency
    {
        // Подтягиваются зависимости из дочернего скоупа, 
        // но лучше так не делать, потому что дочерний скоуп может создаться позже или удалиться раньше текущего
        PlayerDependency _playerDependency;

        public SceneDependency(PlayerDependency playerDependency)
        {
            _playerDependency = playerDependency;
        }


        public void Method()
        {
            Debug.Log("Execute SceneDependency Method");
        }

        public void PlayerMethod()
        {
            Debug.Log("Execute SceneDependency PlayerMethod");
            _playerDependency.Method();
        }
    }
}