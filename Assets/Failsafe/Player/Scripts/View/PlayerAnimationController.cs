using Failsafe.Player.View;
using Failsafe.PlayerMovements;
using Failsafe.PlayerMovements.States;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Failsafe.Player
{
    public class PlayerAnimationController : IInitializable, ITickable, IDisposable
    {
        private readonly PlayerController _playerController;
        private readonly Animator _animator;
        private readonly Transform _payerTransform;

        private float _movementDumpTime = 0.2f;
        private int _xMovementId = Animator.StringToHash("XMovement");
        private int _zMovementId = Animator.StringToHash("ZMovement");
        private int _standingId = Animator.StringToHash("Standing");
        private int _walkingId = Animator.StringToHash("Walking");
        private int _runningId = Animator.StringToHash("Running");
        private int _crouchingId = Animator.StringToHash("Crouching");
        private int _fallingId = Animator.StringToHash("Falling");
        private int _disabledId = Animator.StringToHash("Disabled");
        private int _groundedId = Animator.StringToHash("Grounded");
        private int _jumpId = Animator.StringToHash("Jump");


        public PlayerAnimationController(PlayerController playerController, PlayerView playerView)
        {
            _playerController = playerController;
            _animator = playerView.Animator;
            _payerTransform = playerView.PlayerTransform;
        }

        public void Tick()
        {
            var playerVelocity = _payerTransform.InverseTransformVector(_playerController.PlayerMovementController.Velocity);
            var velocityXZ = new Vector3(playerVelocity.x, 0, playerVelocity.z);
            if (velocityXZ.Equals(Vector3.zero))
            {
                _animator.SetFloat(_xMovementId, 0, _movementDumpTime, Time.deltaTime);
                _animator.SetFloat(_zMovementId, 0, _movementDumpTime, Time.deltaTime);
            }
            else
            {
                velocityXZ.Normalize();
                _animator.SetFloat(_xMovementId, velocityXZ.x, _movementDumpTime, Time.deltaTime);
                _animator.SetFloat(_zMovementId, velocityXZ.z, _movementDumpTime, Time.deltaTime);
            }

            _animator.SetBool(_standingId, _playerController.StateMachine.CurrentState is StandingState);
            _animator.SetBool(_walkingId, _playerController.StateMachine.CurrentState is WalkState || _playerController.StateMachine.CurrentState is StandingState);
            _animator.SetBool(_runningId, _playerController.StateMachine.CurrentState is SprintState);
            _animator.SetBool(_crouchingId, _playerController.StateMachine.CurrentState is CrouchState || _playerController.StateMachine.CurrentState is CrouchIdle);
            _animator.SetBool(_fallingId, _playerController.StateMachine.CurrentState is FallState);
            _animator.SetBool(_groundedId, _playerController.PlayerMovementController.IsGrounded);
        }

        public void Initialize()
        {
            _playerController.StateMachine.GetState<JumpState>().OnEnter += OnStartJumping;
            _playerController.StateMachine.GetState<JumpState>().OnExit += OnFinishJumping;
        }

        public void Dispose()
        {
            _playerController.StateMachine.GetState<JumpState>().OnEnter -= OnStartJumping;
            _playerController.StateMachine.GetState<JumpState>().OnExit -= OnFinishJumping;
        }

        public void OnStartJumping()
        {
            _animator.SetTrigger(_jumpId);
        }

        public void OnFinishJumping()
        {
            //Иногда юнити не успевает сбросить триггер, нужно это делать вручную
            _animator.ResetTrigger(_jumpId);
        }
    }
}
