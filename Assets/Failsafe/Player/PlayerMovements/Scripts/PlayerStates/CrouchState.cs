using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Присядь
    /// </summary>
    public class CrouchState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly Transform _camera;
        private readonly Vector3 _cameraOriginalPosition;
        private readonly PlayerNoiseController _playerNoiseController;
        private float _speed => _movementParametrs.CrouchSpeed;

        public bool CanStand() => true;

        public CrouchState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParameters movementParametrs, Transform camera, PlayerNoiseController playerNoiseController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _camera = camera;
            _cameraOriginalPosition = camera.localPosition;
            _playerNoiseController = playerNoiseController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(CrouchState));
            //TODO: Пока при приседании опускается толко камера, исправить
            _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.CrouchHeight));
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Reduced);
        }

        public override void Update()
        {
            var movement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed;
            _characterController.Move(movement * Time.deltaTime);
            _playerNoiseController.SetNoiseStrength(movement == Vector3.zero ? PlayerNoiseVolume.Minimum : PlayerNoiseVolume.Reduced);
        }

        public override void Exit()
        {
            _camera.localPosition = _cameraOriginalPosition;
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Default);
        }
    }
}
