
using Failsafe.PlayerMovements;
using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Modifiebles;
using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public class Tushkan : StimulatorBaseItem
    {


        public Tushkan(TushkanData data, IEffectManager effectManager, PlayerMovementParameters playerMovementParameters) : base(effectManager)
        {
            Effect = new TushkanEffect(playerMovementParameters, data);
        }




    }

    public class TushkanEffect : StimulatorBaseEffect
    {
        private PlayerMovementParameters _playerMovementParameters;

        public TushkanEffect(PlayerMovementParameters playerMovementParameters, StimulatorBaseData data) : base(data)
        {
            _playerMovementParameters = playerMovementParameters;

        }
        public override void ApplyEffect()
        {
            _playerMovementParameters.JumpMaxHeight.AddModificator(Modificator);
            _playerMovementParameters.JumpMaxSpeed.AddModificator(Modificator);
        }

        public override void ClearEffect()
        {
            _playerMovementParameters.JumpMaxHeight.RemoveModificator(Modificator);
            _playerMovementParameters.JumpMaxSpeed.RemoveModificator(Modificator);
        }
    }
}
