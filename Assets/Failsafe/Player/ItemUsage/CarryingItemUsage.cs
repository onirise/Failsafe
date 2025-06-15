
using Failsafe.Player.Scripts.Interaction;
using Failsafe.PlayerMovements;
using UnityEngine;
using VContainer;

namespace Failsafe.Player.ItemUsage
{
    public class CarryingItemUsage : MonoBehaviour
    {
        private PhysicsInteraction _physicsInteraction;
        private InputHandler _inputHandler;

        [Inject]
        public void Construct(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
        
        private void Start()
        {
            _physicsInteraction = GetComponent<PhysicsInteraction>();
        }

        private void Update()
        {
            if (_physicsInteraction._carryingObject is null) return;
            if (!_inputHandler.UseTriggered) return;
        
            if (_physicsInteraction._carryingObject.TryGetComponent(out Item item))
            {
                item.ActionsGroups.ForEach(group => group.Event.Invoke());
            }
        }
    }
}