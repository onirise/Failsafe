using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Контроллер воздействия гравитации на игрока
    /// </summary>
    public class PlayerGravityController
    {
        private PlayerMovementController _movementController;
        private CharacterController _characterController;
        private PlayerMovementParameters _movementParametrs;
        private float _coyoteTime = 0.1f;
        private float _coyoteTimeProgress = 0f;
        private bool _gravityEnabled = true;

        public bool IsGrounded => _coyoteTimeProgress <= 0;
        public bool IsFalling => _coyoteTimeProgress > _coyoteTime;

        public PlayerGravityController(PlayerMovementController movementController, CharacterController characterController, PlayerMovementParameters movementParametrs)
        {
            _movementController = movementController;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public void HandleGravity()
        {
            if (_gravityEnabled)
            {
                var gravity = new Vector3(0, -_movementParametrs.GravityForce, 0);
                _movementController.SetGravity(gravity);
            }
            else
            {
                _movementController.SetGravity(Vector3.zero);
            }
        }

        public void CheckGrounded()
        {
            if (_characterController.isGrounded)
            {
                _coyoteTimeProgress = 0;
            }
            else
            {
                _coyoteTimeProgress += Time.deltaTime;
            }
        }

        public void DisableGravity()
        {
            _gravityEnabled = false;
        }

        public void EnableGravity()
        {
            _gravityEnabled = true;
        }
    }
}