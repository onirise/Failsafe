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
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly Transform _camera;
        private readonly PlayerRotationController _playerRotationController;
        private readonly Vector3 _cameraOriginalPosition;

        private float _maxSpeed => _movementParametrs.SlideSpeed;
        private float _minSpeed => _movementParametrs.WalkSpeed;
        private float _maxSlideTime => _movementParametrs.MaxSlideTime;
        private float _minSlideTime => _movementParametrs.MinSlideTime;
        private float _slideProgress = 0f;
        public bool SlideFinished() => _slideProgress >= _maxSlideTime;
        public bool CanStand() => _slideProgress >= _minSlideTime;

        public SlideState(
            InputHandler inputHandler,
            CharacterController characterController,
            PlayerMovementParameters movementParametrs,
            Transform camera,
            PlayerRotationController playerRotationController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _camera = camera;
            _playerRotationController = playerRotationController;
            _cameraOriginalPosition = _camera.localPosition;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SlideState));
            _slideProgress = 0f;
            //TODO: Пока при приседании опускается толко камера, исправить
            _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.SlideHeight));
            _playerRotationController.RotateBodyToDirection(_characterController.transform.forward);
        }

        public override void Update()
        {
            _slideProgress += Time.deltaTime;
            var slideMovement = _characterController.transform.forward * GetCurrentSpeed() * Time.deltaTime;
            _characterController.Move(slideMovement);
        }

        public override void Exit()
        {
            _camera.localPosition = _cameraOriginalPosition;
            _playerRotationController.SyncBodyRotationToHead();
            _playerRotationController.RotateBodyToHead();
        }

        private float GetCurrentSpeed() => Mathf.Lerp(_maxSpeed, _minSpeed, _slideProgress / _maxSlideTime);

    }
}
