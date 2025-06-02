using UnityEngine;

namespace Failsafe.PlayerMovements.Controllers
{
    /// <summary>
    /// Контроллер перемещения персонажа
    /// </summary>
    public class PlayerMovementController
    {
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
        /// Задать движение персонажа
        /// Не умножать на Time.deltaTime перед применением движения
        /// </summary>
        /// <param name="motion"></param>
        public void Move(Vector3 motion)
        {
            _movement = motion;
        }

        /// <summary>
        /// Задать силу притяжения
        /// </summary>
        /// <param name="gravity"></param>
        public void AddGravity(Vector3 gravity)
        {
            _gravity = gravity;
        }

        public void HandleMovement()
        {
            var motion = _movement + _gravity;
            _movement = Vector3.zero;
            _gravity = Vector3.zero;
            _characterController.Move(motion * Time.deltaTime);
        }
    }
}