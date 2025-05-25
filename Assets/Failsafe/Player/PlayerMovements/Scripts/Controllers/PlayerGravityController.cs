using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Контроллер воздействия гравитации на игрока
    /// </summary>
    public class PlayerGravityController
    {
        private CharacterController _characterController;
        private PlayerMovementParameters _movementParametrs;
        private float _coyoteTime = 0.1f;
        private float _coyoteTimeProgress = 0f;
        private bool _gravityEnabled = true;

        public bool IsGrounded => _coyoteTimeProgress <= 0;
        public bool IsFalling => _coyoteTimeProgress > _coyoteTime;

        public PlayerGravityController(CharacterController characterController, PlayerMovementParameters movementParametrs)
        {
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public void HandleGravity()
        {
            if (!_gravityEnabled) return;

            var gravity = new Vector3(0, -_movementParametrs.GravityForce, 0) * Time.deltaTime;
            _characterController.Move(gravity);
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