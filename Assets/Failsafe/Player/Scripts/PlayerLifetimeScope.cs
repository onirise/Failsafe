using Failsafe.GameSceneServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.Player
{
    /// <summary>
    /// Регистрация комонентов игрового персонажа
    /// </summary>
    public class PlayerLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerDependency>(Lifetime.Scoped);
            builder.RegisterEntryPoint<TestStart>();
            //TODO: зарегистрировать системы игрока : InputHandler, MovementController ...
        }
    }
    //TODO: Все что ниже используется для теста зависимостей, удалить
    public class PlayerDependency
    {
        public void Method()
        {
            Debug.Log("Execute PlayerDependency Method");
        }
    }

    public class TestStart : IStartable
    {
        // Можно подтягивать зависимости родительского скоупа
        private SceneDependency _dependency;
        private PlayerDependency _playerDependency;

        public TestStart(SceneDependency dependency, PlayerDependency playerDependency)
        {
            _dependency = dependency;
            _playerDependency = playerDependency;

        }


        public void Start()
        {
            Debug.Log("TestStart started");
            _dependency.Method();
            _playerDependency.Method();
            _dependency.PlayerMethod();
        }

    }
}
