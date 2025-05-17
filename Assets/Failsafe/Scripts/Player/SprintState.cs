using UnityEngine;

namespace PlayerStates
{
    public class SprintState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private PlayerMovementParametrs _movementParametrs;

        private float _speed => _movementParametrs.runSpeed;
        private float _sprintProgress = 0f;
        private float _slideAfterTime = 1f;
        
        public bool CanSlide() => _sprintProgress > _slideAfterTime;

        public SprintState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SprintState));
            _sprintProgress = 0f;
        }

        public override void Update()
        {
            _sprintProgress += Time.deltaTime;
            var deltaMovement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed * Time.deltaTime;
            _characterController.Move(deltaMovement);
        }
    }
}
