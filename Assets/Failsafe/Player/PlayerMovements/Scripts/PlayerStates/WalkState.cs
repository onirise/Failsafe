using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Ходьба
    /// </summary>
    public class WalkState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseController _playerNoiseController;

        private float _speed => _movementParametrs.WalkSpeed;

        public WalkState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerNoiseController = playerNoiseController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(WalkState));
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Default);
        }

        public override void Update()
        {
            var movement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed;
            _characterController.Move(movement * Time.deltaTime);
            _playerNoiseController.SetNoiseStrength(movement == Vector3.zero ? PlayerNoiseVolume.Minimum : PlayerNoiseVolume.Default);
        }
    }
}
