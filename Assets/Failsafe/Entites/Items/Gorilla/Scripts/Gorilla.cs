using Failsafe.Player.Model;
using Failsafe.Scripts.EffectSystem;
using Failsafe.Scripts.Modifiebles;
using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public class Gorilla : StimulatorBaseItem
    {

        public Gorilla(GorillaData data, PlayerModelParameters playerModelParameters, IEffectManager effectManager) : base(effectManager)
        {
            Effect = new GorillaEffect(playerModelParameters, data);
        }


    }

    public class GorillaEffect : StimulatorBaseEffect
    {
        private PlayerModelParameters _playerModelParameters;

        public GorillaEffect(PlayerModelParameters playerModelParameters, GorillaData data) : base(data)
        {
            _playerModelParameters = playerModelParameters;
        }
        public override void ApplyEffect()
        {

            _playerModelParameters.ThrowPower.AddModificator(Modificator);
            _playerModelParameters.ThrowTorquePower.AddModificator(Modificator);
        }

        public override void ClearEffect()
        {
            _playerModelParameters.ThrowPower.RemoveModificator(Modificator);
            _playerModelParameters.ThrowTorquePower.RemoveModificator(Modificator);
        }
    }
}