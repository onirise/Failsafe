using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
	public class DeathState : BehaviorForcedState
	{
		public override void Enter()
		{
			base.Enter();
			
			Debug.Log("You are dead");
		}
	}
}