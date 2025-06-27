using Failsafe.Player.Model;
using Failsafe.Player.View;
using Failsafe.PlayerMovements;
using Failsafe.Scripts.Health;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
        [SerializeReference] private PlayerModelParameters _playerModelParameters;
        [SerializeReference] private PlayerMovementParameters _playerMovementParameters;
        [SerializeReference] private PlayerNoiseParameters _playerNoiseParameters;

        [SerializeField] private PlayerView _playerView;
        [SerializeField] private InputActionAsset _inputActionAsset;

        [SerializeField] private PlayerDependenciesConfig _config;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerModelParameters);
            builder.RegisterInstance(_playerMovementParameters);
            builder.RegisterInstance(_playerNoiseParameters);
            builder.RegisterComponent(_playerView);
            builder.RegisterComponent(_inputActionAsset);
            builder.Register<IDisposable, PlayerMovementSpeedModifier>(Lifetime.Scoped).AsSelf();

            builder.Register<InputHandler>(Lifetime.Scoped);

            builder.Register(_config.HealthType, Lifetime.Singleton).As<IHealth>().WithParameter(_playerModelParameters.MaxHealth);
            builder.Register(_config.StaminaType, Lifetime.Singleton).As<IStamina>().WithParameter(_playerModelParameters.MaxStamina);
            builder.RegisterEntryPoint<PlayerDamageable>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayerStaminaController>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<PlayerController>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<PlayerAnimationController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayerCameraController>(Lifetime.Scoped);
        }
    }
}
