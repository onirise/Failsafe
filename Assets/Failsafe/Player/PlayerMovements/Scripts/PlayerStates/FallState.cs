using Failsafe.Scripts.EffectSystem;
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
        private readonly PlayerMovementParameters _movementParameters;
        private readonly PlayerNoiseController _playerNoiseController;
        private readonly IEffectManager _effectManager;
        private readonly SlowOnLandingEffect _slowOnLanding;

        private float _fallProgress = 0;
        private Vector3 _initialVelocity;
        private Vector3 _initialPosition;

        public float FallHeight => _initialPosition.y - _characterController.transform.position.y;

        public FallState(InputHandler inputHandler, CharacterController characterController, PlayerMovementController movementController, PlayerMovementParameters movementParameters, PlayerNoiseController playerNoiseController, IEffectManager effectManager)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementController = movementController;
            _movementParameters = movementParameters;
            _playerNoiseController = playerNoiseController;
            _effectManager = effectManager;
            _slowOnLanding = new SlowOnLandingEffect(_movementParameters);
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(FallState));
            _fallProgress = 0;
            _initialVelocity = new Vector3(_movementController.Velocity.x, 0, _movementController.Velocity.z);
            _initialPosition = _characterController.transform.position;
            _movementController.SetGravity(_movementParameters.InitialGravityStrength * _movementParameters.GravityForce * Vector3.down);
        }

        public override void Update()
        {
            _fallProgress += Time.deltaTime;

            var gravityStrength = Mathf.Lerp(_movementParameters.InitialGravityStrength, 1, _fallProgress / _movementParameters.TimeToFullGravityForce);
            var gravity = _movementParameters.GravityForce * gravityStrength * Vector3.down;
            var airMovement = _movementController.GetRelativeMovement(_inputHandler.MovementInput) * _movementParameters.AirMovementSpeed;

            _movementController.Move(_initialVelocity + airMovement);
            _movementController.SetGravity(gravity);
        }

        public override void Exit()
        {
            _movementController.SetGravityDefault();
            if (FallHeight > _movementParameters.FallDistanceToSlow)
            {
                _effectManager.ApplyEffect(_slowOnLanding);
            }
            _playerNoiseController.CreateNoise(FallHeight, 2);
        }
    }
}
