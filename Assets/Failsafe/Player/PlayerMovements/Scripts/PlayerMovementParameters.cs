using Failsafe.Scripts.Modifiebles;
using UnityEngine;

namespace Failsafe.PlayerMovements
{
    /// <summary>
    /// Параметры настройки движения игрока
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerMovementParameters", menuName = "Parameters/PlayerMovementParameters")]
    public class PlayerMovementParameters : ScriptableObject
    {
        [Header("Скорость передвижения")]
        /// <summary>
        /// Скорость ходьбы
        /// </summary>
        public ModifiableField<float> WalkSpeed = 3.5f;
        /// <summary>
        /// Скорость бега
        /// </summary>
        public ModifiableField<float> RunSpeed = 7f;
        /// <summary>
        /// Скорость ходьбы в присяди
        /// </summary>
        public ModifiableField<float> CrouchSpeed = 1.75f;

        [Header("Скольжение")]
        /// <summary>
        /// Сколько нужно бежать перед скольжением
        /// </summary>
        public float RunDurationBeforeSlide = 0.5f;
        /// <summary>
        /// Скорость скольжения
        /// </summary>
        public float SlideSpeed = 6f;
        /// <summary>
        /// Максимальное время скольжения
        /// </summary>
        public float MaxSlideTime = 1.5f;
        /// <summary>
        /// Минимальное время скольжения
        /// </summary>
        public float MinSlideTime = 1f;

        [Header("Прыжок")]
        /// <summary>
        /// Высота прыжка
        /// </summary>
        public ModifiableField<float> JumpMaxHeight = 1.5f;
        /// <summary>
        /// Максимальная скорость прыжка
        /// </summary>
        public ModifiableField<float> JumpMaxSpeed = 8f;
        /// <summary>
        /// Минимальная длительность прыжка
        /// </summary>
        public float JumpMinDuration = 0.1f;
        /// <summary>
        /// Длительность прыжка
        /// </summary>
        public float JumpDuration = 0.5f;
        /// <summary>
        /// Скорость коректироваки движения в воздухе
        /// </summary>
        public float AirMovementSpeed = 0.5f;

        [Header("Падение")]
        /// <summary>
        /// Сила притяжения
        /// </summary>
        public float GravityForce = 8f;
        /// <summary>
        /// Сила притяжения в начале падения
        /// </summary>
        public float InitialGravityStrength = 0.1f;
        /// <summary>
        /// Время до разгона
        /// </summary>
        public float TimeToFullGravityForce = 0.5f;
        /// <summary>
        /// Высота падения для замедления
        /// </summary>
        public float FallDistanceToSlow = 2f;
        /// <summary>
        /// Множитель скорости
        /// </summary>
        public float SlowMultiplierOnLanding = 0.5f;
        /// <summary>
        /// Длительность замедления
        /// </summary>
        public float SlowDurationOnLanding = 3f;
        /// <summary>
        /// Высота падения для оглушения
        /// </summary>
        public float FallDistanceToDisable = 4f;
        /// <summary>
        /// Длительность оглушения
        /// </summary>
        public float DisableDurationOnLanding = 1f;

        [Header("Зацепление")]
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

        [Header("Перелезание")]
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

        [Header("Залезание")]
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