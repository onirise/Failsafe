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
        private  PlayerNoiseController _playerNoiseController;
        private  StepController _stepController;
        private float Speed => _movementParametrs.WalkSpeed;

        public WalkState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController, StepController stepController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerNoiseController = playerNoiseController;
            _stepController = stepController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(WalkState));
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Default);
            _stepController.Enable(Speed);

        }

        public override void Update()
        {
            var movement = _inputHandler.GetRelativeMovement(_characterController.transform) * Speed;
            _characterController.Move(movement * Time.deltaTime);
            _playerNoiseController.SetNoiseStrength(movement == Vector3.zero ? PlayerNoiseVolume.Minimum : PlayerNoiseVolume.Default);
        }

        public override void Exit()
        {
            _stepController.Disable();
        }


    }
}
