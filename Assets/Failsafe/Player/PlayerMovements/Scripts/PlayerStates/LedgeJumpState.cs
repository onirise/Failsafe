using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Прыжок с выступа
    /// </summary>
    public class LedgeJumpState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly Transform _headTransform;

        private float _jumpForce => _movementParametrs.JumpMaxSpeed * 0.5f;
        private float _jumpForceFade => _movementParametrs.JumpMaxSpeed * 0.5f;
        private float _jumpProgress = 0;
        private Vector3 _initialVelocity;

        public bool OnGround() => _characterController.isGrounded;
        /// <summary>
        /// Находимся в высшей точке прыжка
        /// </summary>
        /// <returns></returns>
        // Формулу нужно подбирать чтобы было красиво
        public bool InHightPoint() => (_jumpForce - _jumpProgress * _jumpForceFade) < _movementParametrs.GravityForce * 0.8;

        public LedgeJumpState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParameters movementParametrs, Transform headTransform)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _headTransform = headTransform;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(LedgeJumpState));
            _jumpProgress = 0;
            _initialVelocity = _headTransform.forward * 5f;
        }

        public override void Update()
        {
            _jumpProgress += Time.deltaTime;
            var jumpMovement = Vector3.up * (_jumpForce - _jumpProgress * _jumpForceFade) * Time.deltaTime;
            _characterController.Move(jumpMovement + _initialVelocity * Time.deltaTime);
        }
    }
}