using UnityEngine;

namespace PlayerStates
{
    /// <summary>
    /// Зацепление за выступ
    /// </summary>
    public class GrabLedgeState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParametrs _movementParametrs;
        private readonly PlayerGravityController _playerGravityController;
        private readonly PlayerRotationController _playerRotationController;
        private readonly Transform _grabPoint;
        private readonly LedgeDetector _obstacleDetector;
        private LedgeData _obstacleData;
        private float _stickSpeed = 2f;

        public GrabLedgeState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementParametrs movementParametrs,
            PlayerGravityController playerGravityController,
            LedgeDetector obstacleDetector,
            PlayerRotationController playerRotationController,
            Transform grabPoint)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerGravityController = playerGravityController;
            _playerRotationController = playerRotationController;
            _obstacleDetector = obstacleDetector;
            _grabPoint = grabPoint;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(GrabLedgeState));
            _obstacleData = _obstacleDetector.LedgeInView;
            _playerGravityController.DisableGravity();
            _playerRotationController.RotateBodyToDirection(-_obstacleData.FrontSideNormal);
            StickToObstacle(_obstacleData);
            _characterController.SimpleMove(Vector3.zero);
        }

        public override void Update()
        {
            _obstacleData = _obstacleDetector.FindLedgeInFront();
            var movementInput = _inputHandler.MovementInput;
            if (movementInput.x != 0)
            {
                var movementX = movementInput.x;
                var checkObstacleDirection = _characterController.transform.right * movementX * 0.2f;
                var sideObstacle = _obstacleDetector.FindLedgeOnDirection(checkObstacleDirection);
                var heghtDiff = sideObstacle.Height - _obstacleData.Height;
                if (Mathf.Abs(heghtDiff) < _movementParametrs.GrabLedgeHeightDifference)
                {
                    var movement = _movementParametrs.GrabLedgeSpeed * movementX * Time.deltaTime * _characterController.transform.right;
                    _characterController.Move(movement);
                    _playerRotationController.RotateBodyToDirection(-_obstacleData.FrontSideNormal);
                    //StickToObstacle(_obstacleData);
                }
            }
        }

        public override void Exit()
        {
            _playerGravityController.EnableGravity();
            if (_inputHandler.MoveForward)
            {
                _playerRotationController.RotateHeadToBody();
            }
            else
            {
                _playerRotationController.RotateBodyToHead();
            }
            _playerRotationController.SyncBodyRotationToHead();
            // TODO фикс метода StickToObstacle
            _characterController.velocity.Set(0, 0, 0);
        }

        private void StickToObstacle(LedgeData obstacle)
        {
            var pathToGrabPoint = obstacle.GrabPointPosition - _grabPoint.position;
            // TODO Этот способ задает большие значения velocity у игрока, исправить перемежение в точку захвата
            _characterController.Move(pathToGrabPoint);
        }
    }
}
