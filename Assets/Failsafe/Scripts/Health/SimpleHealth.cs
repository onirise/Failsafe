using System;
using UnityEngine;

namespace Failsafe.Scripts.Health
{
	public class SimpleHealth : IHealth
	{
		public event Action<float> OnHealthChanged = delegate { };
		public event Action OnDeath = delegate { };
		
		private readonly float maxHealth;
		
		private float health;

		public float MaxHealth => maxHealth;
		public float CurrentHealth => health;
		public bool IsDead => health <= 0 || Mathf.Approximately(health, 0f);

		public SimpleHealth(float maxHealth)
		{
			this.maxHealth = maxHealth;
			
			health = maxHealth;
		}
		
		public void AddHealth(float toAdd)
		{
			if (IsDead)
			{
				return;
			}
			
			health = Mathf.Clamp(health + toAdd, 0f, MaxHealth);
			
			OnHealthChanged.Invoke(health);

			if (IsDead)
			{
				OnDeath();
			}
		}
	}
}