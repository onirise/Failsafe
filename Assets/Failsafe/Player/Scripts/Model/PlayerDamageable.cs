using Failsafe.Player.View;
using Failsafe.Scripts.Damage;
using Failsafe.Scripts.Damage.Implementation;
using Failsafe.Scripts.Damage.Providers;
using Failsafe.Scripts.Health;
using VContainer.Unity;

namespace Failsafe.Player.Scripts.Model
{
    /// <summary>
    /// Получение урона
    /// </summary>
    public class PlayerDamageable : IInitializable
    {
        private readonly IHealth _health;
        private IDamageService _damageService;
        private DamageableComponent _damageableComponent;

        public PlayerDamageable(IHealth health, PlayerView playerView)
        {
            _health = health;
            _damageableComponent = playerView.Damageable;
        }

        public void Initialize()
        {
            _damageService = new DamageService(new FlatDamageProvider(_health));
            _damageableComponent.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(IDamage damage)
        {
            _damageService.Provide(damage);
        }
    }
}
