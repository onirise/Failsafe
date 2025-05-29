using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Открыт инвентарь
    /// </summary>
    public class InventoryState : BehaviorState
    {
        public override void Enter()
        {
            Debug.Log("Enter " + nameof(InventoryState));
        }
        // TODO вызывается когда игрок открывает инвентарь
    }
}
