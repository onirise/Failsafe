using System;
using UnityEngine;

namespace Failsafe.Player.Scripts.Model
{
    /// <summary>
    /// Выносливость персонажа
    /// </summary>
    public interface IStamina
    {
        public event Action<float> OnStaminaSpended;
        public event Action<float> OnStaminaRestored;

        public bool IsEmpty { get; }
        /// <summary>
        /// Максимальное значение выносливости
        /// </summary>
        public float MaxStamina { get; }
        /// <summary>
        /// Текущее значение выносливости
        /// </summary>
        public float CurrentStamina { get; }
        /// <summary>
        /// Потратить выносливость
        /// </summary>
        /// <param name="amount">Кол-во потраченной выносливости</param>
        public void SpendStamina(float amount);
        /// <summary>
        /// Восполнить выносливость
        /// </summary>
        /// <param name="amount">Кол-во восстановленной выносливости</param>
        public void RestoreStamina(float amount);
    }

    /// <summary>
    /// Выносливость персонажа
    /// </summary>
    public class PlayerStamina : IStamina
    {

        public event Action<float> OnStaminaSpended;
        public event Action<float> OnStaminaRestored;

        public bool IsEmpty => _currentStamina <= 0;

        public float MaxStamina { get; private set; }

        public float CurrentStamina => Mathf.Max(0, _currentStamina);
        private float _currentStamina;


        public PlayerStamina(float maxStamina)
        {
            MaxStamina = maxStamina;
            _currentStamina = maxStamina;
        }

        public void RestoreStamina(float amount)
        {
            _currentStamina = Mathf.Min(_currentStamina + amount, MaxStamina);
            OnStaminaRestored?.Invoke(amount);
        }

        public void SpendStamina(float amount)
        {
            // Значение может быть отрицательнмы, чтобы не абузить затратные действия при низкой выносливости
            _currentStamina -= amount;
            OnStaminaSpended?.Invoke(amount);
        }
    }
}
