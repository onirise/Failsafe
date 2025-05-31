using System;
using UnityEngine;

namespace Failsafe.PlayerMovements
{
	[Serializable]
	public class PlayerModelParameters
	{
		[field: SerializeField]
		public float MaxHealth { get; private set; } = 1000f;

		[field: SerializeField]
		public float MaxStamina { get; private set; } = 100f;
	}
}