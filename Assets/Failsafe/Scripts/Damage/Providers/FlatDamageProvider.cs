using Failsafe.Scripts.Damage.Implementation;

namespace Failsafe.Scripts.Damage.Providers
{
    public class FlatDamageProvider : IDamageProvider<FlatDamage>
    {
        private readonly IDamageable _damageable;

        public FlatDamageProvider(IDamageable damageable)
        {
            _damageable = damageable;
        }
        
        public void Provide(FlatDamage damage)
        {
            _damageable.TakeDamage(damage.DamageAmount);
        }
    }
}