using Failsafe.Player.Interaction;

namespace Failsafe.PlayerMovements.States
{
    public class ThrowState : BehaviorState
    {
        private readonly PhysicsInteraction _physicsInteraction;


        public ThrowState(PhysicsInteraction physicsInteraction)
        {
            _physicsInteraction = physicsInteraction;
        }
        
        public override void Enter()
        {
            _physicsInteraction.ThrowObject();
        }
    }
}