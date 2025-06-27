using System;
using UnityEngine;

namespace Failsafe.Player.Model
{
	[CreateAssetMenu(fileName = "PlayerModelParameters", menuName = "Player/Parameters/PlayerModelParameters")]
	public class PlayerModelParameters : ScriptableObject
	{
		[field: SerializeField]
		public float MaxHealth { get; private set; } = 1000f;

		[field: SerializeField]
		public float MaxStamina { get; private set; } = 100f;

		/// <summary>
		/// Пассивная регенерация выносливости
		/// </summary>
		public float RegenerateStaminaPerSecond = 20f;
		/// <summary>
		/// Задержка перед началом регенерации после траты выносливости
		/// </summary>
		public float RegenerationDelay = 1f;
		/// <summary>
		/// Затраты выносливости на прыжок
		/// </summary>
		public float StaminaForJump = 25f;
		/// <summary>
		/// Затраты выносливости на бег в секунду
		/// </summary>
		public float StaminaForRunPerSecond = 20f;
	}
}