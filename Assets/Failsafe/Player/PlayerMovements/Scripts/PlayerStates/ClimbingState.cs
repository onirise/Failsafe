using Failsafe.Obstacles;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Взбирание на уступ
    /// </summary>
    public class ClimbingState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerGravityController _playerGravityController;
        private readonly PlayerLedgeController _playerLedgeController;
        private float _duration = 0.5f;
        private float _climbingProgress = 0f;
        private float _climbSpeed = 10f;
        private Vector3 _targetPosition;

        private LedgeGrabPoint LedgeGrabPoint => !_playerLedgeController.AttachedLedgeGrabPoint.IsEmpty ? _playerLedgeController.AttachedLedgeGrabPoint : _playerLedgeController.LedgeGrabPointInView;

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

        public ClimbingState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementParameters movementParametrs,
            PlayerGravityController playerGravityController,
            PlayerLedgeController playerLedgeController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerGravityController = playerGravityController;
            _playerLedgeController = playerLedgeController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(ClimbingState));
            _climbingProgress = 0;
            _targetPosition = LedgeGrabPoint.Position;
            _playerGravityController.DisableGravity();
        }

        public override void Update()
        {
            _climbingProgress += Time.deltaTime;
            Vector3 movement;
            var hightDiff = _targetPosition.y - _characterController.transform.position.y;
            if (hightDiff < 0)
            {
                movement = Vector3.up * hightDiff;
            }
            else
            {
                movement = _targetPosition - _characterController.transform.position;
            }
            _characterController.Move(_climbSpeed * Time.deltaTime * movement.normalized);
        }

        public override void Exit()
        {
            _playerGravityController.EnableGravity();
            _playerLedgeController.AttachedLedgeGrabPoint = LedgeGrabPoint.Empty;
        }
    }
}
