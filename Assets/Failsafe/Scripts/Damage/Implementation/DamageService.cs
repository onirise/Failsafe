using Failsafe.Scripts.Damage.Providers;
using System;
using System.Collections.Generic;

namespace Failsafe.Scripts.Damage.Implementation
{
    public class DamageService : IDamageService
    {
        private readonly Dictionary<Type, IDamageProvider> _damageProviders = new();

        public DamageService(IDamageProvider damageProvider)
        {
            Register(damageProvider);
        }

        public void Provide(IDamage damage)
        {
            if (!_damageProviders.TryGetValue(damage.GetType(), out var provider))
            {
                throw new ArgumentException($"There is no damage provider for damage type {damage.GetType()}");
            }

            provider.Provide(damage);
        }

        public void Register(IDamageProvider provider)
        {
            _damageProviders.TryAdd(provider.Type, provider);
        }

        public void Unregister(IDamageProvider provider)
        {
            _damageProviders.Remove(provider.Type);
        }
    }
}