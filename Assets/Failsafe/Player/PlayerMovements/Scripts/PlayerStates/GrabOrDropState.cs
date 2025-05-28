using Failsafe.Player.Interaction;

namespace Failsafe.PlayerMovements.States
{
    public class GrabOrDropState : BehaviorState
    {
        private readonly PhysicsInteraction _physicsInteraction;

        public GrabOrDropState(PhysicsInteraction physicsInteraction)
        {
            _physicsInteraction = physicsInteraction;
        }
        
        public override void Enter()
        {
            _physicsInteraction.GrabOrDrop();
        }
    }
}