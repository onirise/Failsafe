using Failsafe.Items;
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
        // Параметры предметов
        [SerializeField] private ScriptableObject[] _playerItemsData;

        [SerializeField] private PlayerView _playerView;
        [SerializeField] private InputActionAsset _inputActionAsset;

        [SerializeField] private Camera _playerCam;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerModelParameters);
            builder.RegisterInstance(_playerMovementParameters);
            builder.RegisterInstance(_playerNoiseParameters);
            builder.RegisterComponent(_playerView);
            builder.RegisterComponent(_inputActionAsset);

            builder.RegisterInstance(_playerCam);

            builder.Register<InputHandler>(Lifetime.Scoped);

            builder.Register<IHealth, PlayerHealth>(Lifetime.Singleton).AsSelf().WithParameter(_playerModelParameters.MaxHealth);
            builder.Register<IStamina, PlayerStamina>(Lifetime.Singleton).AsSelf().WithParameter(_playerModelParameters.MaxStamina);
            builder.RegisterEntryPoint<PlayerDamageable>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayerStaminaController>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<PlayerController>(Lifetime.Scoped).AsSelf();

            builder.Register<PlayerHandsContainer>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayerHandsSystem>(Lifetime.Scoped).AsSelf();
            
            builder.RegisterEntryPoint<PlayerAnimationController>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PlayerCameraController>(Lifetime.Scoped);


            RegisterItems(builder);
        }

        private void RegisterItems(IContainerBuilder builder)
        {
            foreach (var itemData in _playerItemsData)
            {
                builder.RegisterInstance(itemData).As(itemData.GetType());
            }
            builder.Register<Stimpack>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<StasisGun>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<Adrenaline>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<Tushkan>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<Gorilla>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        }
    }
}
