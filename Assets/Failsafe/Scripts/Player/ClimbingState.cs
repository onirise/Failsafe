using UnityEngine;

namespace PlayerStates
{
    /// <summary>
    /// Взбирание на уступ
    /// </summary>
    public class ClimbingState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParametrs _movementParametrs;
        private readonly PlayerGravityController _playerGravityController;
        private readonly Transform _grabPoint;
        private float _duration = 0.5f;
        private float _climbingProgress = 0f;
        private float _climbSpeed = 10f;
        private Vector3 _targetPosition;

        public bool CanClimb()
        {
            var capsuleBottomPoint = _grabPoint.position + _characterController.transform.forward * 0.2f + Vector3.up * 0.51f;
            var collide = Physics.SphereCast(capsuleBottomPoint, 0.5f, Vector3.up, out var hitInfo, 1);
            if (collide)
            {
                Debug.Log("Cant Climb " + hitInfo.point);
                Debug.DrawLine(_grabPoint.position, hitInfo.point, Color.black);
            }
            return !collide;
        }

        public bool ClimbFinish() => _climbingProgress >= _duration;

        public ClimbingState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementParametrs movementParametrs,
            PlayerGravityController playerGravityController,
            Transform grabPoint)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerGravityController = playerGravityController;
            _grabPoint = grabPoint;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(ClimbingState));
            _climbingProgress = 0;
            _targetPosition = _grabPoint.position + _characterController.transform.forward * 0.2f;
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
        }
    }
}
