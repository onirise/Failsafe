
using Failsafe.Scripts.Modifiebles;
using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public class Tushkan : IUsable, ILimitedEffect
    {
        TushkanData _data;
        private PlayerMovements.PlayerMovementParameters _playerMovementParameters;
        private IModificator<float> _jumpModificator;
        public Tushkan(TushkanData data, PlayerMovements.PlayerMovementParameters playerMovementParameters)
        {
            _data = data;
            _playerMovementParameters = playerMovementParameters;
            _jumpModificator = new MultiplierFloat(_data.JumpMultiplier, priority: 100);
        }
        public IEnumerator EndEffect(Action callback)
        {
            yield return new WaitForSeconds(_data.Duration);
            _playerMovementParameters.JumpMaxHeight.RemoveModificator(_jumpModificator);
            _playerMovementParameters.JumpMaxSpeed.RemoveModificator(_jumpModificator);
            callback.Invoke();
        }


        public void Use()
        {
            //происходит именно УМНОЖЕНИЕ
            _playerMovementParameters.JumpMaxHeight.AddModificator(_jumpModificator);
            _playerMovementParameters.JumpMaxSpeed.AddModificator(_jumpModificator);
        }


    }
}
