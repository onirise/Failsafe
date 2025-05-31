using System;

namespace Failsafe.Scripts.Health
{
	public interface IHealth
	{
		event Action<float> OnHealthChanged;
		event Action OnDeath;
		
		float MaxHealth { get; }
		float CurrentHealth { get; }
		bool IsDead { get; }

		void AddHealth(float health);
	}
}