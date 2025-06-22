using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Failsafe.PlayerMovements.Controllers;
using Failsafe.PlayerMovements.States;
using Failsafe.Scripts.Health;
using FMODUnity;
using TMPro;
using VContainer;
using Failsafe.Player.View;
using VContainer.Unity;
using Failsafe.Player.Model;


namespace Failsafe.PlayerMovements
{
    /// <summary>
    /// Движение персонажа
    /// </summary>
    public class PlayerController : IInitializable, ITickable, IFixedTickable
    {
        private readonly PlayerMovementParameters _movementParametrs;
        private readonly PlayerNoiseParameters _noiseParametrs;
        private readonly SignalManager _signalManager;
        private readonly InputHandler _inputHandler;
        private readonly PlayerView _playerView;
        private readonly IHealth _health;
        private readonly IStamina _stamina;
        private readonly PlayerStaminaController _playerStaminaController;
        private readonly PlayerMovementSpeedModifier _playerMovementSpeedModifier;
        private PlayerMovementController _movementController;
        private PlayerRotationController _playerRotationController;
        private PlayerBodyController _playerBodyController;
        private BehaviorStateMachine _behaviorStateMachine;
        private PlayerLedgeController _ledgeController;
        private PlayerNoiseController _noiseController;
        private StepController _stepController;

        public BehaviorStateMachine StateMachine => _behaviorStateMachine;
        public PlayerMovementController PlayerMovementController => _movementController;

        public PlayerController(
            PlayerMovementParameters movementParametrs,
            PlayerNoiseParameters noiseParametrs,
            SignalManager signalManager,
            InputHandler inputHandler,
            PlayerView playerView,
            IHealth health,
            IStamina stamina,
            PlayerStaminaController playerStaminaController,
            PlayerMovementSpeedModifier playerMovementSpeedModifier)
        {
            _movementParametrs = movementParametrs;
            _noiseParametrs = noiseParametrs;
            _signalManager = signalManager;
            _inputHandler = inputHandler;
            _playerView = playerView;
            _health = health;
            _stamina = stamina;
            _playerStaminaController = playerStaminaController;
            _playerMovementSpeedModifier = playerMovementSpeedModifier;
        }

        public void Initialize()
        {
            _movementController = new PlayerMovementController(_playerView.CharacterController, _movementParametrs);
            _playerRotationController = new PlayerRotationController(_playerView.PlayerTransform, _playerView.PlayerRigHead, _inputHandler);
            _playerBodyController = new PlayerBodyController(_playerView.CharacterController);
            _ledgeController = new PlayerLedgeController(_playerView.PlayerTransform, _playerView.PlayerCamera, _playerView.PlayerGrabPoint, _movementParametrs);
            _noiseController = new PlayerNoiseController(_playerView.PlayerTransform, _noiseParametrs, _signalManager);
            _stepController = new StepController(_playerView.CharacterController, _movementParametrs, _playerView.FootstepEvent);

            InitializeStateMachine();
        }



        private void InitializeStateMachine()
        {
            var deathState = new DeathState();
            var forcedStates = new List<BehaviorForcedState>
            {
                 deathState
            };
            _behaviorStateMachine = new BehaviorStateMachine(forcedStates);

            var standingState = new StandingState(_inputHandler, _movementController);
            var walkState = new WalkState(_inputHandler, _movementController, _movementParametrs, _noiseController, _stepController);
            var runState = new SprintState(_inputHandler, _movementController, _movementParametrs, _noiseController, _stepController, _playerStaminaController);
            var slideState = new SlideState(_inputHandler, _movementController, _movementParametrs, _playerBodyController, _playerRotationController);
            var crouchState = new CrouchState(_inputHandler, _movementController, _movementParametrs, _playerBodyController, _noiseController, _stepController);
            var jumpState = new JumpState(_inputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerStaminaController);
            var fallState = new FallState(_inputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _noiseController, _playerMovementSpeedModifier);
            var grabLedgeState = new GrabLedgeState(_inputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _playerRotationController, _ledgeController);
            var climbingUpState = new ClimbingUpState(_inputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _ledgeController);
            var climbingOnState = new ClimbingOnState(_inputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _ledgeController);
            var climbingOverState = new ClimbingOverState(_inputHandler, _playerView.CharacterController, _movementController, _movementParametrs, _ledgeController);
            var ledgeJumpState = new LedgeJumpState(_inputHandler, _playerView.CharacterController, _movementParametrs, _playerView.PlayerCamera);
            var crouchIdleState = new CrouchIdle(_playerBodyController, _movementController, _movementParametrs, _noiseController, _stepController);


            Func<bool> runStatePrecondition = () => _inputHandler.MoveForward && _inputHandler.SprintTriggered && !_stamina.IsEmpty;
            Func<bool> jumpStatePrecondition = () => _inputHandler.JumpTriggered && !_stamina.IsEmpty && _movementController.IsGroundedFor(0.1f);

            standingState.AddTransition(walkState, () => !_inputHandler.MovementInput.Equals(Vector2.zero));
            standingState.AddTransition(crouchIdleState, () => _inputHandler.CrouchTrigger.IsTriggered, _inputHandler.CrouchTrigger.ReleaseTrigger);
            standingState.AddTransition(climbingOverState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            standingState.AddTransition(climbingOnState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            standingState.AddTransition(jumpState, () => jumpStatePrecondition());

            walkState.AddTransition(runState, () => runStatePrecondition());
            walkState.AddTransition(climbingOverState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            walkState.AddTransition(climbingOnState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            walkState.AddTransition(jumpState, () => jumpStatePrecondition());
            walkState.AddTransition(crouchState, () => _inputHandler.CrouchTrigger.IsTriggered, _inputHandler.CrouchTrigger.ReleaseTrigger);
            walkState.AddTransition(fallState, () => _movementController.IsFalling);
            walkState.AddTransition(standingState, () => _inputHandler.MovementInput.Equals(Vector2.zero));

            runState.AddTransition(walkState, () => !runStatePrecondition());
            runState.AddTransition(climbingOverState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            runState.AddTransition(climbingOnState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            runState.AddTransition(jumpState, () => jumpStatePrecondition());
            runState.AddTransition(slideState, () => _inputHandler.CrouchTrigger.IsTriggered && runState.CanSlide(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            runState.AddTransition(fallState, () => _movementController.IsFalling);

            slideState.AddTransition(crouchState, () => slideState.SlideFinished());
            slideState.AddTransition(walkState, () => _inputHandler.CrouchTrigger.IsTriggered && slideState.CanStand() && _playerBodyController.CanStand(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            slideState.AddTransition(fallState, () => _movementController.IsFalling);

            crouchState.AddTransition(runState, () => runStatePrecondition() && _playerBodyController.CanStand());
            crouchState.AddTransition(walkState, () => _inputHandler.CrouchTrigger.IsTriggered && _playerBodyController.CanStand(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            crouchState.AddTransition(fallState, () => _movementController.IsFalling);
            crouchState.AddTransition(crouchIdleState, () => _inputHandler.MovementInput.Equals(Vector2.zero));
            crouchState.AddTransition(jumpState, () => jumpStatePrecondition());

            crouchIdleState.AddTransition(crouchState, () => !_inputHandler.MovementInput.Equals(Vector2.zero));
            crouchIdleState.AddTransition(standingState, () => _inputHandler.CrouchTrigger.IsTriggered && _playerBodyController.CanStand(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            crouchIdleState.AddTransition(jumpState, () => jumpStatePrecondition());

            jumpState.AddTransition(runState, () => runStatePrecondition() && jumpState.CanGround() && _movementController.IsGrounded);
            jumpState.AddTransition(walkState, () => jumpState.CanGround() && _movementController.IsGrounded);
            jumpState.AddTransition(fallState, jumpState.InHightPoint);
            jumpState.AddTransition(grabLedgeState, () => _inputHandler.GrabLedgeTrigger.IsTriggered && _ledgeController.CanGrabToLedgeGrabPointInView());

            fallState.AddTransition(walkState, () => _movementController.IsGrounded);
            fallState.AddTransition(grabLedgeState, () => _inputHandler.GrabLedgeTrigger.IsTriggered && _ledgeController.CanGrabToLedgeGrabPointInView());

            grabLedgeState.AddTransition(fallState, () => _inputHandler.MoveBack && grabLedgeState.CanFinish());
            grabLedgeState.AddTransition(climbingUpState, () => _inputHandler.MoveForward && grabLedgeState.CanFinish() && climbingUpState.CanClimb());
            grabLedgeState.AddTransition(ledgeJumpState, () => _inputHandler.JumpTriggered && grabLedgeState.CanFinish());

            ledgeJumpState.AddTransition(grabLedgeState, () => _ledgeController.CanGrabToLedgeGrabPointInView());
            ledgeJumpState.AddTransition(fallState, ledgeJumpState.InHightPoint);

            climbingUpState.AddTransition(walkState, () => climbingUpState.ClimbFinish());
            climbingOnState.AddTransition(walkState, () => climbingOnState.ClimbFinish());
            climbingOverState.AddTransition(fallState, () => climbingOverState.ClimbFinish());

            _behaviorStateMachine.SetInitState(walkState);

        }

        public void Tick()
        {
            _ledgeController.HandleFindingLedge();
            _playerRotationController.HandlePlayerRotation();
            _behaviorStateMachine.Update();
            _stepController.Update();
            _playerMovementSpeedModifier.Update();
            if (_health.IsDead)
            {
                _behaviorStateMachine.ForseChangeState<DeathState>();
            }
        }

        public void FixedTick()
        {
            _movementController.HandleMovement();
            _movementController.CheckGrounded();
        }
    }
}
