using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Падение
    /// </summary>
    public class FallState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseController _playerNoiseController;

        //Если не задавать дополнительную силу падения то контроллер не приземляется
        private float _fallSpeed = 0.1f;
        private float _fallProgress = 0;
        private Vector3 _initialVelocity;
        private Vector3 _initialPosition;

        public FallState(InputHandler inputHandler, CharacterController characterController, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
            _movementParametrs = movementParametrs;
            _playerNoiseController = playerNoiseController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(FallState));
            _fallProgress = 0;
            _initialVelocity = new Vector3(_movementController.Velocity.x, -_fallSpeed, _movementController.Velocity.z);
            _initialPosition = _characterController.transform.position;
        }

        public override void Update()
        {
            _fallProgress += Time.deltaTime;
            _movementController.Move(_initialVelocity);
        }

        public override void Exit()
        {
            var fallHeight = _initialPosition.y - _characterController.transform.position.y;
            if (fallHeight > 3f)
            {
                Debug.Log("Ай, больно в ноге");
            }
            _playerNoiseController.CreateNoise(fallHeight, 2);
        }
    }
}
