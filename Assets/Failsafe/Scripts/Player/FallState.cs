using UnityEngine;

namespace PlayerStates
{
    public class FallState : BehaviorState
    {
        private InputHandler _inputHandler;
        private CharacterController _characterController;
        private readonly PlayerMovementParametrs _movementParametrs;

        //Если не задавать дополнительную силу падения то контроллер не приземляется
        private float _fallSpeed = 0.1f;
        private float _fallProgress = 0;
        private Vector3 _initialVelocity;

        public bool OnGround() => _characterController.isGrounded;

        public FallState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParametrs movementParametrs)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(FallState));
            _fallProgress = 0;
            _initialVelocity = new Vector3(_characterController.velocity.x, -_fallSpeed, _characterController.velocity.z);
        }

        public override void Update()
        {
            _fallProgress += Time.deltaTime;
            _characterController.Move(_initialVelocity * Time.deltaTime);
        }

        public override void Exit()
        {
            if (_fallProgress > 1f)
            {
                Debug.Log("Ай, больно в ноге");
            }
        }
    }
}
