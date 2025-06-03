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
        private readonly PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly Transform _camera;
        private readonly Vector3 _cameraOriginalPosition;
        private readonly PlayerNoiseController _playerNoiseController;
        private readonly StepController _stepController;
        private float Speed => _movementParametrs.CrouchSpeed;

        public bool CanStand() => true;

        public CrouchState(InputHandler inputHandler, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, Transform camera, PlayerNoiseController playerNoiseController , StepController stepController)
        {
            _inputHandler = inputHandler;
            _movementController = movementController;
            _movementParametrs = movementParametrs;
            _camera = camera;
            _cameraOriginalPosition = camera.localPosition;
            _playerNoiseController = playerNoiseController;
            _stepController = stepController;
            
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(CrouchState));
            //TODO: Пока при приседании опускается толко камера, исправить
            _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.CrouchHeight));
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Reduced);
            _stepController.Enable(Speed);
        }

        public override void Update()
        {
            var movement = _movementController.GetRelativeMovement(_inputHandler.MovementInput) * Speed;
            _movementController.Move(movement);
            _playerNoiseController.SetNoiseStrength(movement == Vector3.zero ? PlayerNoiseVolume.Minimum : PlayerNoiseVolume.Reduced);
        }

        public override void Exit()
        {
            _camera.localPosition = _cameraOriginalPosition;
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Default);
            _stepController.Disable();
        }
    }
}
