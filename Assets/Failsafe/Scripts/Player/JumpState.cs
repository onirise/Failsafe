using UnityEngine;

namespace PlayerStates
{
    public class JumpState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private readonly PlayerMovementParametrs _movementParametrs;

        private float _jumpForce => _movementParametrs.jumpForce;
        private float _jumpForceFade => _movementParametrs.jumpForceFade;
        private float _jumpProgress = 0;
        private Vector3 _initialVelocity;

        public bool OnGround() => _characterController.isGrounded;

        public JumpState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(JumpState));
            _jumpProgress = 0;
            _initialVelocity = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z);
        }

        public override void Update()
        {
            _jumpProgress += Time.deltaTime;
            var jumpMovement = Vector3.up * (_jumpForce - _jumpProgress * _jumpForceFade) * Time.deltaTime;
            _characterController.Move(jumpMovement + _initialVelocity * Time.deltaTime);
        }
    }
}
