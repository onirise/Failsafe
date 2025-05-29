using Failsafe.Player.Interaction;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    public class ThrowState : BehaviorState
    {
        private readonly PhysicsInteraction _physicsInteraction;
        private float _throwForceMultiplier;
        private float _maxForceMultiplier = 3f;

        public ThrowState(PhysicsInteraction physicsInteraction)
        {
            _physicsInteraction = physicsInteraction;
        }
        
        public override void Enter()
        {
            _throwForceMultiplier = 0f;
        }

        public override void Update()
        {
            _throwForceMultiplier += Time.deltaTime;
            _throwForceMultiplier = Mathf.Clamp(_throwForceMultiplier, 0f, _maxForceMultiplier);
        }

        public override void Exit()
        {
            _physicsInteraction.ThrowObject(_throwForceMultiplier);
        }
    }
}