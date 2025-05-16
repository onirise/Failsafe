using UnityEngine;

namespace PlayerStates
{
    public class SprintState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private PlayerMovementParametrs _movementParametrs;

        private float _speed => _movementParametrs.runSpeed;

        public SprintState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SprintState));
        }

        public override void Update()
        {
            var deltaMovement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed * Time.deltaTime;
            _characterController.Move(deltaMovement);
        }
    }
}
