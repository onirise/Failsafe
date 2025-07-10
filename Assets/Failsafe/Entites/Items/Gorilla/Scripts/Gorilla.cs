using Failsafe.Player.Model;
using Failsafe.Scripts.Modifiebles;
using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public class Gorilla : IUsable, ILimitedEffect
    {
        GorillaData _data;
        private PlayerModelParameters _playerModelParameters;
        private IModificator<float> _throwPowerModificator;
        public Gorilla(GorillaData data, PlayerModelParameters playerModelParameters)
        {
            _data = data;
            _playerModelParameters = playerModelParameters;
            _throwPowerModificator = new MultiplierFloat(_data.ThrowPowerMultiplier, priority: 100);
        }
        public IEnumerator EndEffect(Action callback)
        {
            yield return new WaitForSeconds(_data.Duration);
            _playerModelParameters.ThrowPower.RemoveModificator(_throwPowerModificator);
            _playerModelParameters.ThrowTorquePower.RemoveModificator(_throwPowerModificator);
            callback.Invoke();
        }


        public void Use()
        {
            //происходит именно УМНОЖЕНИЕ
            _playerModelParameters.ThrowPower.AddModificator(_throwPowerModificator);
            _playerModelParameters.ThrowTorquePower.AddModificator(_throwPowerModificator);
        }
    }
}