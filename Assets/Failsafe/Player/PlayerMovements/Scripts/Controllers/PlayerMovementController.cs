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
        private Vector3 _movement;
        private Vector3 _gravity;

        public PlayerMovementController(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public Vector3 GetRelativeMovement(Vector2 inputMovement)
        {
            return Vector3.ClampMagnitude(inputMovement.x * _characterController.transform.right + inputMovement.y * _characterController.transform.forward, 1);
        }

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

        public void HandleMovement()
        {
            var motion = _movement + _gravity;
            _characterController.Move(motion * Time.deltaTime);
            Velocity = _characterController.velocity;
        }
    }
}