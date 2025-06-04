using System;

namespace Failsafe.Scripts.Damage
{
    public interface IDamageProvider
    {
        Type Type { get; }

        void Provide(IDamage damage);
    }
    
    public interface IDamageProvider<in T> : IDamageProvider where T : IDamage
    {
        Type IDamageProvider.Type => typeof(T);

        void IDamageProvider.Provide(IDamage damage)
        {
            if (damage is not T tDamage)
            {
                throw new ArgumentException($"Damage is not type of {typeof(T)}");
            }
            
            Provide(tDamage);
        }

        void Provide(T damage);
    }
}