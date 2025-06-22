using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Контроллер перемещения персонажа
    /// </summary>
    public class PlayerMovementController
    {
        public Vector3 Velocity { get; private set; }
        private readonly CharacterController _characterController;
        private readonly PlayerMovementParameters _playerMovementParameters;
        private Vector3 _movement;
        private Vector3 _gravity;

        private float _coyoteTime = 0.1f;
        private float _coyoteTimeProgress = 0f;
        private float _groundedAt;
        private bool _gravityEnabled = true;

        /// <summary>
        /// Находится на земле
        /// </summary>
        public bool IsGrounded => _coyoteTimeProgress <= 0;
        /// <summary>
        /// Находится на земле некоторое время
        /// </summary>
        /// <param name="duration">Время в секундах</param>
        /// <returns></returns>
        public bool IsGroundedFor(float duration) => _groundedAt + duration <= Time.time;

        public bool IsFalling => _coyoteTimeProgress > _coyoteTime;

        public PlayerMovementController(CharacterController characterController, PlayerMovementParameters playerMovementParameters)
        {
            _characterController = characterController;
            _playerMovementParameters = playerMovementParameters;
        }

        public Vector3 GetRelativeMovement(Vector2 inputMovement)
        {
            return Vector3.ClampMagnitude(inputMovement.x * _characterController.transform.right + inputMovement.y * _characterController.transform.forward, 1);
        }

        /// <summary>
        /// Определить находится на земле или нет
        /// </summary>


        /// <summary>
        /// Задать движение персонажа. Перемещение выполнится методом <see cref="HandleMovement"/>
        /// </summary>
        /// <remarks>
        /// Предыдущее заданое движение сохраняется, т.е. чтобы остановить персонажа нужно явно задать перемещение <see cref="Vector3.zero"/>
        /// <para/>Не умножать на <see cref="Time.deltaTime"/> перед применением движения
        /// </remarks>
        /// <param name="motion">Перемещение</param>
        public void Move(Vector3 motion)
        {
            _movement = motion;
        }

        /// <summary>
        /// Задать силу притяжения
        /// </summary>
        /// <param name="gravity"></param>
        public void SetGravity(Vector3 gravity)
        {
            _gravity = gravity;
        }

        /// <summary>
        /// Задать силу притяжения стандартным значением
        /// </summary>
        public void SetGravityDefault()
        {
            SetGravity(_playerMovementParameters.GravityForce * Vector3.down);
        }

        public void HandleMovement()
        {
            var motion = _movement + _gravity;
            _characterController.Move(motion * Time.deltaTime);
            Velocity = _characterController.velocity;
        }

        public void CheckGrounded()
        {
            if (_characterController.isGrounded)
            {
                if (_coyoteTimeProgress > 0)
                {
                    _coyoteTimeProgress = 0;
                    _groundedAt = Time.time;
                }
            }
            else
            {
                _coyoteTimeProgress += Time.deltaTime;
            }
        }
    }
}