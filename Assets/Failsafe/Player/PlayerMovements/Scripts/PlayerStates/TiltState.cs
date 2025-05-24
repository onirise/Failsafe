using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Наклон в сторону
    /// </summary>
    public class TiltState : BehaviorState
    {
        public override void Enter()
        {
            Debug.Log("Enter " + nameof(TiltState));
        }
    }
}
