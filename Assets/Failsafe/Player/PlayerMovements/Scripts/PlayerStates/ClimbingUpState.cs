using Failsafe.Obstacles;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Подтягивание на уступ
    /// </summary>
    public class ClimbingUpState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParameters;
        private readonly PlayerLedgeController _playerLedgeController;
        private float _duration = 0.5f;
        private float _climbingProgress = 0f;
        private float _climbSpeed = 10f;
        private Vector3 _targetPosition;

        private LedgeGrabPoint LedgeGrabPoint => !_playerLedgeController.AttachedLedgeGrabPoint.IsEmpty
            ? _playerLedgeController.AttachedLedgeGrabPoint
            : _playerLedgeController.LedgeGrabPointInView;

        public bool CanClimb()
        {
            var capsuleBottomPoint = LedgeGrabPoint.Position + Vector3.up * 0.51f;
            var collide = Physics.SphereCast(capsuleBottomPoint, 0.5f, Vector3.up, out var hitInfo, 1);
            if (collide)
            {
                Debug.Log("Cant Climb " + hitInfo.point);
                Debug.DrawLine(LedgeGrabPoint.Position, hitInfo.point, Color.black);
            }
            return !collide;
        }

        public bool ClimbFinish() => _climbingProgress >= _duration;

        public ClimbingUpState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementController movementController,
            PlayerMovementParameters movementParameters,
            PlayerLedgeController playerLedgeController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
            _movementParameters = movementParameters;
            _playerLedgeController = playerLedgeController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(ClimbingUpState));
            _climbingProgress = 0;
            _targetPosition = LedgeGrabPoint.Position;
            _movementController.SetGravity(Vector3.zero);
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
            _movementController.Move(_climbSpeed * movement);
        }

        public override void Exit()
        {
            _movementController.SetGravityDefault();
            _playerLedgeController.AttachedLedgeGrabPoint = LedgeGrabPoint.Empty;
        }
    }
}
