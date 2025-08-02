using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    public class StandingState : BehaviorState
    {
        private InputHandler _inputHandler;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerRotationController _playerRotationController;

        public StandingState(InputHandler inputHandler, PlayerMovementController movementController, PlayerRotationController playerRotationController)
        {
            _inputHandler = inputHandler;
            _movementController = movementController;
            _playerRotationController = playerRotationController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(StandingState));
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
            _playerRotationController.RotateBodyToHead();
            _playerRotationController.SyncBodyRotationToHead();
            base.Exit();
        }
    }
}
