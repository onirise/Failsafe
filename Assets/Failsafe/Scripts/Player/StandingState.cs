using UnityEngine;

namespace PlayerStates
{
    //
    public class StandingState : BehaviorState
    {
        private InputHandler _inputHandler;

        public StandingState(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(StandingState));
        }
    }
}
