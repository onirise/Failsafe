using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    public class CrouchIdle : BehaviorState
    {
        private readonly PlayerBodyController _playerBodyController;
        private readonly PlayerMovementController _movementController;
        private readonly Vector3 _cameraOriginalPosition;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseController _playerNoiseController;
        private readonly StepController _stepController;
        private readonly PlayerRotationController _playerRotationController;

        public CrouchIdle(PlayerBodyController playerBodyController, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController, StepController stepController, PlayerRotationController playerRotationController)
        {
            _playerBodyController = playerBodyController;
            _movementController = movementController;
            _movementParametrs = movementParametrs;
            _playerNoiseController = playerNoiseController;
            _stepController = stepController;
            _playerRotationController = playerRotationController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(CrouchIdle));
            _playerBodyController.Crouch();
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Minimum);
            _stepController.Disable();
            _movementController.Move(Vector3.zero);
            _playerRotationController.RotateBodyToDirection(_playerRotationController.HeadDirection);
            base.Enter();
        }

        public override void Update()
        {
            if (Mathf.Abs(_playerRotationController.HeadLocalRotation.y) >= 80)
            {
                _playerRotationController.RotateBodyToHead();
            }
        }

        public override void Exit()
        {
            _playerBodyController.Stand();
            _playerRotationController.RotateBodyToHead();
            _playerRotationController.SyncBodyRotationToHead();
            base.Exit();
        }
    }
}