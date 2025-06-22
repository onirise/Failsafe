using Failsafe.Obstacles;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Перелезание через узкое препятствие
    /// </summary>
    public class ClimbingOverState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerLedgeController _playerLedgeController;

        private LedgeGrabPoint _ledgeGrabPoint;

        private Vector3 _targetPosition;
        private float _duration = 0.3f;
        private float _climbingProgress = 0f;
        private float _climbSpeed = 8f;

        public bool ClimbFinish() => _climbingProgress >= _duration;

        public ClimbingOverState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementController movementController,
            PlayerMovementParameters movementParametrs,
            PlayerLedgeController playerLedgeController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
            _movementParametrs = movementParametrs;
            _playerLedgeController = playerLedgeController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(ClimbingOverState));
            _climbingProgress = 0;
            _ledgeGrabPoint = _playerLedgeController.LedgeGrabPointInFrontBottom;
            _targetPosition = _ledgeGrabPoint.Position + (_ledgeGrabPoint.Width + 0.5f) * -_ledgeGrabPoint.Normal;
            _movementController.SetGravity(Vector3.zero);
            base.Enter();
        }

        public override void Update()
        {
            _climbingProgress += Time.deltaTime;
            Vector3 movement = Vector3.zero;
            var hightDiff = _targetPosition.y - _characterController.transform.position.y;
            if (hightDiff > 0)
            {
                movement = Vector3.up;
            }
            movement += _targetPosition - _characterController.transform.position;
            _movementController.Move(_climbSpeed * movement.normalized);
        }

        public override void Exit()
        {
            _movementController.SetGravity(_movementParametrs.GravityForce * Vector3.down);
            base.Exit();
        }
    }
}
