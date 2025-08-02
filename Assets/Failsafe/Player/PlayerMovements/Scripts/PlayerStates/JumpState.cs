using Failsafe.Player.Model;
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
        private readonly PlayerMovementParameters _movementParameters;
        private readonly PlayerStaminaController _playerStaminaController;
        private float _jumpProgress = 0;
        private Vector3 _initialVelocity;
        private float _targetHeight;

        //Минимальное время прыжка, нужно чтобы не дергало между прыжком и ходьбой. Нужно найти решение лучше
        public bool CanGround() => _jumpProgress >= _movementParameters.JumpMinDuration;
        /// <summary>
        /// Находимся в высшей точке прыжка
        /// </summary>
        /// <returns></returns>
        public bool InHightPoint() => IsCollidedAbove() || _jumpProgress >= _movementParameters.JumpDuration;

        private bool IsCollidedAbove() => (_characterController.collisionFlags & CollisionFlags.CollidedAbove) != 0;

        public JumpState(InputHandler inputHandler, CharacterController characterController, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, PlayerStaminaController playerStaminaController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
            _movementParameters = movementParametrs;
            _playerStaminaController = playerStaminaController;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter " + nameof(JumpState));
            _jumpProgress = 0;
            _initialVelocity = new Vector3(_movementController.Velocity.x, 0, _movementController.Velocity.z);
            _targetHeight = _characterController.transform.position.y + _movementParameters.JumpMaxHeight;
            _playerStaminaController.SpendOnJump();
        }

        public override void Update()
        {
            _jumpProgress += Time.deltaTime;

            var deltaHeight = _targetHeight - _characterController.transform.position.y;
            var upForce = deltaHeight * _movementParameters.GravityForce;
            upForce = Mathf.Min(upForce, _movementParameters.JumpMaxSpeed);
            var jumpMovement = Vector3.up * (upForce + _movementParameters.GravityForce);//Добавляем GravityForce к силе прыжка чтобы компенсировать гравитацию

            var airMovement = _movementController.GetRelativeMovement(_inputHandler.MovementInput) * _movementParameters.AirMovementSpeed;

            _movementController.Move(jumpMovement + _initialVelocity + airMovement);
        }
    }
}
