using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Скольжение
    /// </summary>
    public class SlideState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerBodyController _playerBodyController;
        private readonly PlayerRotationController _playerRotationController;

        private float _maxSpeed => _movementParametrs.SlideSpeed;
        private float _minSpeed => _movementParametrs.WalkSpeed;
        private float _maxSlideTime => _movementParametrs.MaxSlideTime;
        private float _minSlideTime => _movementParametrs.MinSlideTime;
        private float _slideProgress = 0f;
        public bool SlideFinished() => _slideProgress >= _maxSlideTime;
        public bool CanFinish() => _slideProgress >= _minSlideTime;

        public SlideState(
            InputHandler inputHandler,
            PlayerMovementController movementController,
            PlayerMovementParameters movementParametrs,
            PlayerBodyController playerBodyController,
            PlayerRotationController playerRotationController)
        {
            _inputHandler = inputHandler;
            _movementController = movementController;
            _movementParametrs = movementParametrs;
            _playerBodyController = playerBodyController;
            _playerRotationController = playerRotationController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SlideState));
            _slideProgress = 0f;
            _playerBodyController.Slide();
            _playerRotationController.RotateBodyToDirection(_movementController.GetRelativeMovement(Vector2.up));
        }

        public override void Update()
        {
            _slideProgress += Time.deltaTime;
            var movement = _movementController.GetRelativeMovement(Vector2.up) * GetCurrentSpeed();
            _movementController.Move(movement);
        }

        public override void Exit()
        {
            _playerBodyController.Stand();
            _playerRotationController.SyncBodyRotationToHead();
            _playerRotationController.RotateBodyToHead();
        }

        private float GetCurrentSpeed() => Mathf.Lerp(_maxSpeed, _minSpeed, _slideProgress / _maxSlideTime);

    }
}
