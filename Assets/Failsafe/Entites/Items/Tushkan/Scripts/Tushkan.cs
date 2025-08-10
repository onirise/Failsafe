
using Failsafe.PlayerMovements;
using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Modifiebles;
using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public class Tushkan : IUsable
    {
        TushkanData _data;
        IEffectManager _effectManager;
        TushkanEffect _effect;

        public Tushkan(TushkanData data, PlayerMovementParameters playerMovementParameters, IEffectManager effectManager)
        {
            _data = data;
            _effectManager = effectManager;
            _effect = new TushkanEffect(playerMovementParameters, data);
        }



        public ItemUseResult Use()
        {
            _effectManager.ApplyEffect(_effect);
            return ItemUseResult.Consumed;
        }


    }

    public class TushkanEffect : Effect
    {
        private PlayerMovementParameters _playerMovementParameters;
        private IModificator<float> _jumpModificator;
        public TushkanEffect(PlayerMovementParameters playerMovementParameters, TushkanData data)
        {
            _playerMovementParameters = playerMovementParameters;
            _jumpModificator = new MultiplierFloat(data.JumpMultiplier, priority: 100);
            _duration = data.Duration;
            IsUniqueEffect = true;
        }
        public override void ApplyEffect()
        {
            _playerMovementParameters.JumpMaxHeight.AddModificator(_jumpModificator);
            _playerMovementParameters.JumpMaxSpeed.AddModificator(_jumpModificator);
        }

        public override void ClearEffect()
        {
            _playerMovementParameters.JumpMaxHeight.RemoveModificator(_jumpModificator);
            _playerMovementParameters.JumpMaxSpeed.RemoveModificator(_jumpModificator);
        }
    }
}
