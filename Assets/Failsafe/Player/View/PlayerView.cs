using Failsafe.Scripts.Damage.Implementation;
using FMODUnity;
using UnityEngine;

namespace Failsafe.Player.View
{
    /// <summary>
    /// Представление персонажа
    /// </summary>
    /// <remarks>
    /// Должен содержать компоненты специфичные для движка Unity: Рендер, Анимации, Звук.
    /// Логика должна быть вынесена в отдельные Модели и Контроллеры
    /// </remarks>
    public class PlayerView : MonoBehaviour
    {
        /// <summary>
        /// Игровой персонаж
        /// </summary>
        public Transform PlayerTransform;
        /// <summary>
        /// Голова модели персонажа
        /// </summary>
        public Transform PlayerModelHead;
        /// <summary>
        /// Голова рига персонажа
        /// </summary>
        /// <remarks>
        /// Задает поворот камеры и головы модели
        /// </remarks>
        public Transform PlayerRigHead;
        /// <summary>
        /// Камера персонажа
        /// </summary>
        public Transform PlayerCamera;
        /// <summary>
        /// Тело персонажа
        /// </summary>
        public Transform Body;
        /// <summary>
        /// Место для предмета в правой руке
        /// </summary>
        public Transform RightHandItemPlace;

        public Animator Animator;

        public DamageableComponent Damageable;
        /// <summary>
        /// Точка захвата
        /// </summary>
        public Transform PlayerGrabPoint;

        public CharacterController CharacterController;

        public EventReference FootstepEvent;


        void OnValidate()
        {
            if (PlayerTransform == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(PlayerTransform)}");
            if (PlayerModelHead == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(PlayerModelHead)}");
            if (PlayerRigHead == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(PlayerRigHead)}");
            if (PlayerCamera == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(PlayerCamera)}");
            if (RightHandItemPlace == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(RightHandItemPlace)}");
            if (Animator == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(Animator)}");
            if (PlayerGrabPoint == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(PlayerGrabPoint)}");
            if (CharacterController == null)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(CharacterController)}");
            if (FootstepEvent.IsNull)
                Debug.LogWarning($"Не задан компонент {nameof(PlayerView)}.{nameof(FootstepEvent)}");
        }
    }

}
