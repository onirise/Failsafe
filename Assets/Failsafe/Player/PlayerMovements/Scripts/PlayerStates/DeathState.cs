using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
	public class DeathState : BehaviorState
	{
		public override void Enter()
		{
			base.Enter();
			
			Debug.Log("You are dead");
		}
	}
}