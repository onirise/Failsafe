using System;
namespace Failsafe.PlayerMovements
{
    /// <summary>
    /// Параметры настройки движения игрока
    /// </summary>
    public class PlayerMovementParameters
    {
        /// <summary>
        /// Скорость ходьбы
        /// </summary>
        public float WalkSpeed = 8f;
        /// <summary>
        /// Скорость бега
        /// </summary>
        public float RunSpeed = 15f;
        /// <summary>
        /// Скорость ходьбы в присяди
        /// </summary>
        public float CrouchSpeed = 3f;
        /// <summary>
        /// Высота в присяди относительно нормальной высоты
        /// </summary>
        public float CrouchHeight = 0.5f;
        /// <summary>
        /// Скорость скольжения
        /// </summary>
        public float SlideSpeed = 12f;
        /// <summary>
        /// Высота в скольжении относительно нормальной высоты
        /// </summary>
        public float SlideHeight = 0.3f;
        /// <summary>
        /// Максимальное время скольжения
        /// </summary>
        public float MaxSlideTime = 1f;
        /// <summary>
        /// Минимальное время скольжения
        /// </summary>
        public float MinSlideTime = 0.5f;
        /// <summary>
        /// Сила прыжка
        /// </summary>
        public float JumpForce = 20f;
        /// <summary>
        /// Сила угасания прыжка
        /// </summary>
        public float JumpForceFade = 30f;
        /// <summary>
        /// Сила притяжения
        /// </summary>
        public float GravityForce = 8f;
        /// <summary>
        /// Скорость передвижения при зацепдении
        /// </summary>
        public float GrabLedgeSpeed = 4f;
        /// <summary>
        /// На какую высоту игрок может подняться/опуститься пока движется по выступу
        /// </summary>
        public float GrabLedgeHeightDifference = 0.1f;
    }
}