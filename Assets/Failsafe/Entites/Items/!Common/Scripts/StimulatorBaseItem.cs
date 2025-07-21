
using Failsafe.PlayerMovements;
using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Modifiebles;
using UnityEngine;


namespace Failsafe.Items
{
    public class StimulatorBaseItem : IUsable
    {

        IEffectManager _effectManager;
        protected StimulatorBaseEffect Effect;
        public void Use()
        {
            // Если эффекты должны складываться друг с другом, то нужно убрать у эффекта IsUniqueEffect = true;
            // и создавать новый экземпляр перед каждым применением (в этом случае можно создать Pooling для оптимизации)
            _effectManager.ApplyEffect(Effect);
        }

        public StimulatorBaseItem(IEffectManager effectManager)
        {

            _effectManager = effectManager;
        }


    }

    public class StimulatorBaseEffect : Effect
    {

        protected IModificator<float> Modificator;

        public StimulatorBaseEffect(StimulatorBaseData data)
        {
            Modificator = new MultiplierFloat(data.Multiplier, priority: 100);
            _duration = data.Duration;
            IsUniqueEffect = true;
        }
        public override void ApplyEffect()
        {

        }

        public override void ClearEffect()
        {

        }
    }
}
