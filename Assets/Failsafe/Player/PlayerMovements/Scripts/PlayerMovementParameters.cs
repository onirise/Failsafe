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
        /// Расстояние до точки за
        /// </summary>
        public float GrabLedgeMaxDistance = 0.5f;
        /// <summary>
        /// Минимальная высота препятствия, за которое можно ухватиться
        /// </summary>
        public float GrabLedgeMinHeight = 2f;
        /// <summary>
        /// Скорость передвижения при зацепдении
        /// </summary>
        public float GrabLedgeSpeed = 4f;
        /// <summary>
        /// На какую высоту игрок может подняться/опуститься пока движется по выступу
        /// </summary>
        public float GrabLedgeHeightDifference = 0.1f;

        /// <summary>
        /// Расстояние до препятствия, с которого можно начать перелезать
        /// </summary>
        public float ClimbOverMaxDistanceToLedge = 1f;
        /// <summary>
        /// Максимальная ширина препятствия, через которое можно перелезать
        /// </summary>
        public float ClimbOverLedgeMaxWidth = 0.5f;
        /// <summary>
        /// Максимальная высота препятствия, через которое можно перелезать
        /// </summary>
        public float ClimbOverLedgeMaxHeight = 1.5f;

        /// <summary>
        /// Расстояние до препятствия, с которого можно начать залезать
        /// </summary>
        public float ClimbOnMaxDistanceToLedge = 1f;
        /// <summary>
        /// Максимальная высота препятствия, на которое можно залезть
        /// </summary>
        public float ClimbOnLedgeMaxHeight = 2f;
    }
}