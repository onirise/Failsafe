using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    public class StandingState : BehaviorState
    {
        private InputHandler _inputHandler;
        private readonly PlayerMovementController _movementController;

        public StandingState(InputHandler inputHandler, PlayerMovementController movementController)
        {
            _inputHandler = inputHandler;
            _movementController = movementController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(StandingState));
            _movementController.Move(Vector3.zero);
        }
    }
}
