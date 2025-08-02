using Failsafe.Player.Model;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

namespace Failsafe.PlayerMovements.States
{
    /// <summary>
    /// Бег
    /// </summary>
    public class SprintState : BehaviorState
    {
        private InputHandler _inputHandler;
        private readonly PlayerMovementController _movementController;
        private PlayerMovementParameters _movementParameters;
        private readonly PlayerNoiseController _playerNoiseController;
        private StepController _stepController;
        private readonly PlayerStaminaController _playerStaminaController;

        private float Speed => _movementParameters.RunSpeed;
        private float _sprintProgress = 0f;

        public bool CanSlide() => _sprintProgress > _movementParameters.RunDurationBeforeSlide;

        public SprintState(InputHandler inputHandler, PlayerMovementController movementController, PlayerMovementParameters movementParameters, PlayerNoiseController playerNoiseController, StepController stepController, PlayerStaminaController playerStaminaController)
        {
            _inputHandler = inputHandler;
            _movementController = movementController;
            _movementParameters = movementParameters;
            _playerNoiseController = playerNoiseController;
            _stepController = stepController;
            _playerStaminaController = playerStaminaController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SprintState));
            _sprintProgress = 0f;
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Increased);
            _stepController.Enable(Speed);
        }

        public override void Update()
        {
            _sprintProgress += Time.deltaTime;
            var movement = _movementController.GetRelativeMovement(_inputHandler.MovementInput) * Speed;
            _movementController.Move(movement);
            _playerStaminaController.SpendOnRunning();
        }

        public override void Exit()
        {
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Default);
            _stepController.Disable();
        }
    }
}
