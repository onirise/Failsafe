using UnityEngine;

namespace PlayerStates
{
    public class CrouchState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParametrs _movementParametrs;
        private readonly Transform _camera;
        private readonly Vector3 _cameraOriginalPosition;
        private float _speed => _movementParametrs.crouchSpeed;

        public bool CanStand() => true;

        public CrouchState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs, Transform camera)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _camera = camera;
            _cameraOriginalPosition = camera.localPosition;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(CrouchState));
            //TODO: Пока при приседании опускается толко камера, исправить
            _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.crouchHeight));
        }

        public override void Update()
        {
            var deltaMovement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed * Time.deltaTime;
            _characterController.Move(deltaMovement);
        }

        public override void Exit()
        {
            _camera.localPosition = _cameraOriginalPosition;
        }
    }
}
