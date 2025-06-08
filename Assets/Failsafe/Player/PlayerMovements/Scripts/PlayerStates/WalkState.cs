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
        private PlayerMovementController _movementController;
        private PlayerMovementParameters _movementParametrs;
        private  PlayerNoiseController _playerNoiseController;
        private  StepController _stepController;
        private float Speed => _movementParametrs.WalkSpeed;

        public WalkState(InputHandler inputHandler, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController, StepController stepController)
        {
            _inputHandler = inputHandler;
            _movementController = movementController;
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
            var movement = _movementController.GetRelativeMovement(_inputHandler.MovementInput) * Speed;
            _movementController.Move(movement);
            _playerNoiseController.SetNoiseStrength(movement == Vector3.zero ? PlayerNoiseVolume.Minimum : PlayerNoiseVolume.Default);
        }

        public override void Exit()
        {
            _stepController.Disable();
        }


    }
}
