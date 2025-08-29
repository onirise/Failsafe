using Cysharp.Threading.Tasks;
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
        private readonly PlayerHandsSystem _playerHandsSystem;
        private readonly Animator _animator;
        private readonly Transform _payerTransform;
        private readonly int _upperBodyLayerId;
        private int _upperBodyActive;

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
        private int _slidingId = Animator.StringToHash("Sliding");
        private int _healId = Animator.StringToHash("Heal");

        public PlayerAnimationController(PlayerController playerController, PlayerView playerView, PlayerHandsSystem playerHandsSystem)
        {
            _playerController = playerController;
            _animator = playerView.Animator;
            _payerTransform = playerView.PlayerTransform;
            _playerHandsSystem = playerHandsSystem;

            _upperBodyLayerId = _animator.GetLayerIndex("UpperBody");
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
            _animator.SetBool(_slidingId, _playerController.StateMachine.CurrentState is SlideState);
        }

        public void Initialize()
        {
            _playerController.StateMachine.GetState<JumpState>().OnEnter += OnStartJumping;
            _playerController.StateMachine.GetState<JumpState>().OnExit += OnFinishJumping;
            _playerHandsSystem.OnItemStartUsing += OnUseItem;
        }

        public void Dispose()
        {
            _playerController.StateMachine.GetState<JumpState>().OnEnter -= OnStartJumping;
            _playerController.StateMachine.GetState<JumpState>().OnExit -= OnFinishJumping;
            _playerHandsSystem.OnItemStartUsing -= OnUseItem;
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

        public void OnUseItem(ItemType itemType)
        {
            // TODO: Выбрать анимацию по типу предмета
            // Кроме типа нужны другие параметры: 
            // - основное или дополнительное действие
            // - в зависимости от режима предмета могут быть разные анимации
            // - для каждого предмета могут быть разные анимации или разная длительность анимации
            _animator.SetTrigger(_healId);
            ActivateLayerForSeconds(_upperBodyLayerId, 2.5f).Forget();
        }

        private async UniTask ActivateLayerForSeconds(int layerId, float seconds)
        {
            _upperBodyActive++;
            _animator.SetLayerWeight(layerId, 1);
            await UniTask.Delay(TimeSpan.FromSeconds(seconds));
            _upperBodyActive--;
            if (_upperBodyActive == 0)
            {
                _animator.SetLayerWeight(layerId, 0);
            }
        }
    }
}
