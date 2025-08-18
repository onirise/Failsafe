using Failsafe.Scripts.Health;
using Failsafe.Scripts.Modifiebles;

namespace Failsafe.Items
{
    public class Stimpack : IUsable
    {
        private PlayerHealth _playerHealth;
        private StimpackData _data;
        private AdderFloat _maxHealthModificator;

        public Stimpack(PlayerHealth playerHealth, StimpackData data)
        {
            _playerHealth = playerHealth;
            _data = data;
            _maxHealthModificator = new AdderFloat(_data.MaxHealthBonus);
        }

        public ItemUseResult Use()
        {
            _playerHealth.AddHealth(_data.HealAmount);
            _playerHealth.ModifyMaxHealth(_maxHealthModificator);
            return ItemUseResult.Consumed;
        }
    }
}
