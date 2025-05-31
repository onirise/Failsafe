using Failsafe.Scripts.Damage.Implementation;
using Failsafe.Scripts.Health;

namespace Failsafe.Scripts.Damage.Providers
{
    public class FlatDamageProvider : IDamageProvider<FlatDamage>
    {
        private readonly IHealth health;

        public FlatDamageProvider(IHealth health)
        {
            this.health = health;
        }
        
        public void Provide(FlatDamage damage)
        {
            health.AddHealth(-damage.DamageAmount);
        }
    }
}