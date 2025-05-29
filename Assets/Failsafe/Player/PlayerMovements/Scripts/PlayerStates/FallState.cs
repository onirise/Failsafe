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
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseController _playerNoiseController;

        //Если не задавать дополнительную силу падения то контроллер не приземляется
        private float _fallSpeed = 0.1f;
        private float _fallProgress = 0;
        private Vector3 _initialVelocity;
        private Vector3 _initialPosition;

        public bool OnGround() => _characterController.isGrounded;

        public FallState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerNoiseController = playerNoiseController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(FallState));
            _fallProgress = 0;
            _initialVelocity = new Vector3(_characterController.velocity.x, -_fallSpeed, _characterController.velocity.z);
            _initialPosition = _characterController.transform.position;
        }

        public override void Update()
        {
            _fallProgress += Time.deltaTime;
            _characterController.Move(_initialVelocity * Time.deltaTime);
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
