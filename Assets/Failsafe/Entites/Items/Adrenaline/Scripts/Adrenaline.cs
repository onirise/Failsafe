using Failsafe.Scripts.Modifiebles;
using Failsafe.Scripts.EffectSystem;
using Failsafe.PlayerMovements;

namespace Failsafe.Items
{
    public class Adrenaline : IUsable
    {
        private readonly AdrenalineData _data;
        private readonly IEffectManager _effectManager;

        //Т.к. эффект уникальный, можно создать его один раз и не пересоздавать при каждом применении
        private readonly AdrenalineEffect _effect;


        public Adrenaline(AdrenalineData data, PlayerMovementParameters playerMovementParameters, IEffectManager effectManager)
        {
            _data = data;
            _effectManager = effectManager;
            _effect = new AdrenalineEffect(playerMovementParameters, data);
        }

        public ItemUseResult Use()
        {
            // Если эффекты должны складываться друг с другом, то нужно убрать у эффекта IsUniqueEffect = true;
            // и создавать новый экземпляр перед каждым применением (в этом случае можно создать Pooling для оптимизации)
            _effectManager.ApplyEffect(_effect);
            return ItemUseResult.Consumed;
        }

        //TODO: Вынести в отдельный файл если эффект будет переиспользоваться другими предметами
        public class AdrenalineEffect : Effect
        {
            private readonly PlayerMovementParameters _playerMovementParameters;
            private readonly IModificator<float> _speedModificator;

            public AdrenalineEffect(PlayerMovementParameters playerMovementParameters, AdrenalineData data)
            {
                _playerMovementParameters = playerMovementParameters;
                _speedModificator = new MultiplierFloat(data.SpeedMultiplier, priority: 100);
                _duration = data.Duration;
                IsUniqueEffect = true;
            }

            public override void ApplyEffect()
            {
                //происходит именно УМНОЖЕНИЕ
                _playerMovementParameters.WalkSpeed.AddModificator(_speedModificator);
                _playerMovementParameters.RunSpeed.AddModificator(_speedModificator);
                _playerMovementParameters.CrouchSpeed.AddModificator(_speedModificator);
            }

            public override void ClearEffect()
            {
                _playerMovementParameters.WalkSpeed.RemoveModificator(_speedModificator);
                _playerMovementParameters.RunSpeed.RemoveModificator(_speedModificator);
                _playerMovementParameters.CrouchSpeed.RemoveModificator(_speedModificator);
            }
        }
    }
}
