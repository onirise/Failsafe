using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Прыжок
    /// </summary>
    public class JumpState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParametrs;

        private float _jumpForce => _movementParametrs.JumpForce;
        private float _jumpForceFade => _movementParametrs.JumpForceFade;
        private float _jumpProgress = 0;
        private Vector3 _initialVelocity;

        public bool OnGround() => _characterController.isGrounded;
        /// <summary>
        /// Находимся в высшей точке прыжка
        /// </summary>
        /// <returns></returns>
        // Формулу нужно подбирать чтобы было красиво
        public bool InHightPoint() => (_jumpForce - _jumpProgress * _jumpForceFade) < _movementParametrs.GravityForce * 0.8;

        public JumpState(InputHandler inputHandler, CharacterController characterController, PlayerMovementController movementController, PlayerMovementParameters movementParametrs)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
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
            var jumpMovement = Vector3.up * (_jumpForce - _jumpProgress * _jumpForceFade);
            _movementController.Move(jumpMovement + _initialVelocity);
        }
    }
}
