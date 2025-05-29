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
        private CharacterController _characterController;
        private PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseController _playerNoiseController;

        private float _speed => _movementParametrs.RunSpeed;
        private float _sprintProgress = 0f;
        private float _slideAfterTime = 1f;

        public bool CanSlide() => _sprintProgress > _slideAfterTime;

        public SprintState(InputHandler inputHandler, CharacterController characterController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController)
        {
            _inputHandler = inputHandler;
            _characterController = characterController;
            _movementParametrs = movementParametrs;
            _playerNoiseController = playerNoiseController;
        }

        public override void Enter()
        {
            Debug.Log("Enter " + nameof(SprintState));
            _sprintProgress = 0f;
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Increased);
        }

        public override void Update()
        {
            _sprintProgress += Time.deltaTime;
            var deltaMovement = _inputHandler.GetRelativeMovement(_characterController.transform) * _speed * Time.deltaTime;
            _characterController.Move(deltaMovement);
        }

        public override void Exit()
        {
            _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Default);
        }
    }
}
