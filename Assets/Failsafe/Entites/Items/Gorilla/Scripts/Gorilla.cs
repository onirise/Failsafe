using Failsafe.Player.Model;
using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Modifiebles;
using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public class Gorilla : IUsable
    {
        GorillaData _data;

        IEffectManager _effectManager;
        GorillaEffect _effect;
        public Gorilla(GorillaData data, PlayerModelParameters playerModelParameters, IEffectManager effectManager)
        {
            _data = data;
            _effectManager = effectManager;
            _effect = new GorillaEffect(playerModelParameters, data);
        }


        public ItemUseResult Use()
        {
            _effectManager.ApplyEffect(_effect);
            return ItemUseResult.Consumed;
        }
    }

    public class GorillaEffect : Effect
    {
        private PlayerModelParameters _playerModelParameters;
        private IModificator<float> _throwPowerModificator;
        public GorillaEffect(PlayerModelParameters playerModelParameters, GorillaData data)
        {
            _playerModelParameters = playerModelParameters;
            _throwPowerModificator = new MultiplierFloat(data.ThrowPowerMultiplier, priority: 100);
            _duration = data.Duration;
            IsUniqueEffect = true;
        }
        public override void ApplyEffect()
        {

            _playerModelParameters.ThrowPower.AddModificator(_throwPowerModificator);
            _playerModelParameters.ThrowTorquePower.AddModificator(_throwPowerModificator);
        }

        public override void ClearEffect()
        {
            _playerModelParameters.ThrowPower.RemoveModificator(_throwPowerModificator);
            _playerModelParameters.ThrowTorquePower.RemoveModificator(_throwPowerModificator);
        }
    }
}