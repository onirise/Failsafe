using Failsafe.Player.Scripts.Interaction;
using UnityEngine;
using VContainer;
using System.Linq;
using Sirenix.Utilities;
using System.Collections.Generic;
using System;

namespace Failsafe.Player.ItemUsage
{
    [Obsolete("Использовалоьсь для теста. Заменено на PlayerHandsSystem")]
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
            if (!(_inputHandler.UseTrigger.IsTriggered || _inputHandler.ZoomTriggered)) return;
            if (!_physicsInteraction.CarryingObject) return;

            if (_physicsInteraction.CarryingObject.TryGetComponent(out Item item))
            {
                foreach (var action in _inputHandler.PerformedActions)
                    //if(action != false)
                    item.ActionsGroups.Where(x => x.Actions.FirstOrDefault(z => z.action.id == action.id))?.ForEach(x => x.Invoke());
            }
        }
    }
}