using UnityEngine;

namespace PlayerStates
{
    public class SlideState : BehaviorState
    {
        private readonly InputHandler _inputHandler;
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParametrs _movementParametrs;
        private readonly Transform _camera;
        private readonly Vector3 _cameraOriginalPosition;

        private float _maxSpeed => _movementParametrs.runSpeed;
        private float _minSpeed => _movementParametrs.walkSpeed;
        private float _slideTime => _movementParametrs.slideTime;
        private float _slideProgress = 0f;
        public bool SlideFinished() => _slideProgress >= _slideTime;

        public SlideState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs, Transform camera)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _camera = camera;
            _cameraOriginalPosition = _camera.localPosition;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SlideState));
            _slideProgress = 0f;
            //TODO: Пока при приседании опускается толко камера, исправить
            _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.slideHeight));
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
        }

        private float GetCurrentSpeed() => Mathf.Lerp(_maxSpeed, _minSpeed, _slideProgress / _slideTime);

    }
}
