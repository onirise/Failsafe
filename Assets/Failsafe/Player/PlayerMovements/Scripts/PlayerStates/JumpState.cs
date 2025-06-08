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

        //Минимальное время прыжка, нужно чтобы не дергало между прыжком и ходьбой. Нужно найти решение лучше
        public bool CanGround() => _jumpProgress > 0.1f;
        /// <summary>
        /// Находимся в высшей точке прыжка
        /// </summary>
        /// <returns></returns>
        // Формулу нужно подбирать чтобы было красиво
        public bool InHightPoint() => IsCollidedAbove() || (_jumpForce - _jumpProgress * _jumpForceFade) < _movementParametrs.GravityForce * 0.8;

        private bool IsCollidedAbove() => (_characterController.collisionFlags & CollisionFlags.CollidedAbove) != 0;

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
            _initialVelocity = new Vector3(_movementController.Velocity.x, 0, _movementController.Velocity.z);
        }

        public override void Update()
        {
            _jumpProgress += Time.deltaTime;
            var jumpMovement = Vector3.up * (_jumpForce - _jumpProgress * _jumpForceFade);
            _movementController.Move(jumpMovement + _initialVelocity);
        }
    }
}
