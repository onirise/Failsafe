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
        private readonly ObstacleDetector _obstacleDetector;
        private float _distanceToObstacle = 0.75f;
        private float _stickSpeed = 10f;

        public GrabLedgeState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementParametrs movementParametrs,
            PlayerGravityController playerGravityController,
            ObstacleDetector obstacleDetector,
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
            _playerGravityController.DisableGravity();
            _playerRotationController.RotateBodyToDirection(-_obstacleDetector.Obstacle.FrontSideNormal);
            StickToObstacle(_obstacleDetector.Obstacle);
        }

        public override void Update()
        {
            var movementInput = _inputHandler.MovementInput;
            if (movementInput.x != 0)
            {
                var movementX = movementInput.x;
                var checkObstacleDirection = _characterController.transform.right * movementX * 0.5f;
                var sideObstacle = _obstacleDetector.FindObstacleOnDirection(checkObstacleDirection);
                var heghtDiff = sideObstacle.Height - _obstacleDetector.Obstacle.Height;
                if (Mathf.Abs(heghtDiff) < _movementParametrs.GrabLedgeHeightDifference)
                {
                    var movement = _movementParametrs.GrabLedgeSpeed * movementX * Time.deltaTime * _characterController.transform.right;
                    _characterController.Move(movement);
                    // TODO Сделать плавный поворот, сейчас дергается на границах между нормалями
                    _playerRotationController.RotateBodyToDirection(-_obstacleDetector.Obstacle.FrontSideNormal);
                }

            }
            StickToObstacle(_obstacleDetector.Obstacle);
        }

        public override void Exit()
        {
            _playerGravityController.EnableGravity();
            _playerRotationController.RotateBodyToHead();
            _playerRotationController.SyncBodyRotationToHead();
        }

        private void StickToObstacle(ObstacleData obstacle)
        {
            var distanceDiff = obstacle.Distance - _distanceToObstacle;
            var heightDiff = obstacle.Height - _grabPoint.localPosition.y;
            var pathToGrapPosition = Vector3.up * heightDiff + _characterController.transform.forward * distanceDiff;
            _characterController.Move(_stickSpeed * Time.deltaTime * pathToGrapPosition);
        }
    }
}
