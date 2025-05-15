using UnityEngine;

namespace PlayerStates
{
    public class WalkState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private PlayerMovementParametrs _movementParametrs;

        private float _speed => _movementParametrs.walkSpeed;

        public WalkState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(WalkState));
        }

        public override void Update()
        {
            var deltaMovement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed * Time.deltaTime;
            _characterController.Move(deltaMovement);
        }
    }
}
