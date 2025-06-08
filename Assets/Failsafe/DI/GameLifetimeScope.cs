using Failsafe.PlayerMovements;
using Failsafe.Scripts.Health;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Failsafe.DI
{
    public class GameLifetimeScope : LifetimeScope
    {   
        [SerializeField] private PlayerModelParameters playerParameters;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SimpleHealth>(Lifetime.Singleton).As<IHealth>().WithParameter(playerParameters.MaxHealth);
        }
    }
}