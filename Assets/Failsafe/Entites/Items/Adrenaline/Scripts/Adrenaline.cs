using Failsafe.Player.Model;
using UnityEngine;
using Failsafe.Scripts.Modifiebles;
using System.Collections;
using System;

namespace Failsafe.Items
{
    public class Adrenaline : IUsable, ILimitedEffect
    {
        AdrenalineData _data;
        private PlayerMovements.PlayerMovementParameters _playerMovementParameters;
        private IModificator<float> _speedModificator;

        public Adrenaline(AdrenalineData data, PlayerMovements.PlayerMovementParameters playerMovementParameters)
        {
            _data = data;
            _playerMovementParameters = playerMovementParameters;
            _speedModificator = new MultiplierFloat(_data.SpeedMultiplier, priority: 100);
        }

        public IEnumerator EndEffect(Action callback)
        {
            yield return new WaitForSeconds(_data.Duration);
            _playerMovementParameters.WalkSpeed.RemoveModificator(_speedModificator);
            _playerMovementParameters.RunSpeed.RemoveModificator(_speedModificator);
            _playerMovementParameters.CrouchSpeed.RemoveModificator(_speedModificator);

            callback.Invoke();
        }


        public void Use()
        {
            //происходит именно УМНОЖЕНИЕ
            _playerMovementParameters.WalkSpeed.AddModificator(_speedModificator);
            _playerMovementParameters.RunSpeed.AddModificator(_speedModificator);
            _playerMovementParameters.CrouchSpeed.AddModificator(_speedModificator);

        }
    }
}
