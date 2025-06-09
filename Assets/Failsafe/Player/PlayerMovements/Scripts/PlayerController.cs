using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Failsafe.PlayerMovements.Controllers;
using Failsafe.PlayerMovements.States;
using Failsafe.Scripts.Damage;
using Failsafe.Scripts.Damage.Implementation;
using Failsafe.Scripts.Damage.Providers;
using Failsafe.Scripts.Health;
using FMODUnity;
using Failsafe.Player.Interaction;


namespace Failsafe.PlayerMovements
{
    /// <summary>
    /// Движение персонажа
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement params")]
        [SerializeReference] private PlayerMovementParameters _movementParametrs = new PlayerMovementParameters();
        [SerializeField] private InputActionAsset _inputActionAsset;

        [Header("Noise params")]
        [SerializeReference] private PlayerNoiseParameters _noiseParametrs = new PlayerNoiseParameters();

        [Header("Model params")]
        [SerializeReference] private PlayerModelParameters _modelParameters = new();

        private Transform _playerCamera;
        private Transform _playerGrabPoint;

        private DamageableComponent _damageableComponent;

        private IHealth _health;
        private IDamageService _damageService;

        private CharacterController _characterController;
        private PlayerMovementController _movementController;
        private PlayerRotationController _playerRotationController;
        private PlayerBodyController _playerBodyController;
        private BehaviorStateMachine _behaviorStateMachine;
        private InputHandler _inputHandler;
        private PlayerLedgeController _ledgeController;
        private PlayerGravityController _playerGravity;
        private PlayerNoiseController _noiseController;
        public InputHandler InputHandler => _inputHandler;


        [SerializeField] private EventReference _footstepEvent;
        private StepController _stepController;

        private void Awake()
        {
            _health = new SimpleHealth(_modelParameters.MaxHealth);

            _damageService = CreateDamageService();

            _damageableComponent = transform.Find("Capsule").GetComponent<DamageableComponent>();

        }

        private void OnEnable()
        {
            _damageableComponent.OnTakeDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            _damageableComponent.OnTakeDamage -= OnTakeDamage;
        }

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _movementController = new PlayerMovementController(_characterController);
            _playerCamera = transform.Find("Camera");
            _playerGrabPoint = transform.Find("ObstacleGrabPoint");
            _inputHandler = new InputHandler(_inputActionAsset);
            _playerRotationController = new PlayerRotationController(transform, _playerCamera, _inputHandler);
            _playerBodyController = new PlayerBodyController(_playerCamera, _characterController, transform.Find("Capsule"), this);
            _ledgeController = new PlayerLedgeController(transform, _playerCamera, _playerGrabPoint, _movementParametrs);
            _playerGravity = new PlayerGravityController(_movementController, _characterController, _movementParametrs);
            _noiseController = new PlayerNoiseController(transform, _noiseParametrs);
            _stepController = new StepController(_characterController, _movementParametrs, _footstepEvent);

            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            var standingState = new StandingState(_inputHandler, _movementController);
            var walkState = new WalkState(_inputHandler, _movementController, _movementParametrs, _noiseController, _stepController);
            var runState = new SprintState(_inputHandler, _movementController, _movementParametrs, _noiseController, _stepController);
            var slideState = new SlideState(_inputHandler, _movementController, _movementParametrs, _playerBodyController, _playerRotationController);
            var crouchState = new CrouchState(_inputHandler, _movementController, _movementParametrs, _playerBodyController, _noiseController, _stepController);
            var jumpState = new JumpState(_inputHandler, _characterController, _movementController, _movementParametrs);
            var fallState = new FallState(_inputHandler, _characterController, _movementController, _movementParametrs, _noiseController);
            var grabLedgeState = new GrabLedgeState(_inputHandler, _characterController, _movementController, _movementParametrs, _playerGravity, _playerRotationController, _ledgeController);
            var climbingUpState = new ClimbingUpState(_inputHandler, _characterController, _movementController, _movementParametrs, _playerGravity, _ledgeController);
            var climbingOnState = new ClimbingOnState(_inputHandler, _characterController, _movementController, _movementParametrs, _playerGravity, _ledgeController);
            var climbingOverState = new ClimbingOverState(_inputHandler, _characterController, _movementController, _movementParametrs, _playerGravity, _ledgeController);
            var ledgeJumpState = new LedgeJumpState(_inputHandler, _characterController, _movementParametrs, _playerCamera);
            var crouchIdleState = new CrouchIdle(_playerBodyController, _movementController, _movementParametrs, _noiseController, _stepController);

            var deathState = new DeathState();
            var forcedStates = new List<BehaviorForcedState>
            {
                 deathState
            };

            standingState.AddTransition(walkState, () => !_inputHandler.MovementInput.Equals(Vector2.zero));
            standingState.AddTransition(crouchIdleState, () => _inputHandler.CrouchTrigger.IsTriggered, _inputHandler.CrouchTrigger.ReleaseTrigger);
            standingState.AddTransition(climbingOverState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            standingState.AddTransition(climbingOnState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            standingState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);

            walkState.AddTransition(runState, () => _inputHandler.MoveForward && _inputHandler.SprintTriggered);
            walkState.AddTransition(climbingOverState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            walkState.AddTransition(climbingOnState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            walkState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
            walkState.AddTransition(crouchState, () => _inputHandler.CrouchTrigger.IsTriggered, _inputHandler.CrouchTrigger.ReleaseTrigger);
            walkState.AddTransition(fallState, () => _playerGravity.IsFalling);
            walkState.AddTransition(standingState, () => _inputHandler.MovementInput.Equals(Vector2.zero));

            runState.AddTransition(walkState, () => !(_inputHandler.MoveForward && _inputHandler.SprintTriggered));
            runState.AddTransition(climbingOverState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOverLedge());
            runState.AddTransition(climbingOnState, () => _inputHandler.JumpTriggered && _ledgeController.CanClimbOnLedge());
            runState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
            runState.AddTransition(slideState, () => _inputHandler.CrouchTrigger.IsTriggered && runState.CanSlide(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            runState.AddTransition(fallState, () => _playerGravity.IsFalling);

            slideState.AddTransition(crouchState, () => slideState.SlideFinished());
            slideState.AddTransition(walkState, () => _inputHandler.CrouchTrigger.IsTriggered && slideState.CanStand() && _playerBodyController.CanStand(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            slideState.AddTransition(fallState, () => _playerGravity.IsFalling);

            crouchState.AddTransition(runState, () => _inputHandler.MoveForward && _inputHandler.SprintTriggered && _playerBodyController.CanStand());
            crouchState.AddTransition(walkState, () => _inputHandler.CrouchTrigger.IsTriggered && _playerBodyController.CanStand(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            crouchState.AddTransition(fallState, () => _playerGravity.IsFalling);
            crouchState.AddTransition(crouchIdleState, () => _inputHandler.MovementInput.Equals(Vector2.zero));
            crouchState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);

            crouchIdleState.AddTransition(crouchState, () => !_inputHandler.MovementInput.Equals(Vector2.zero));
            crouchIdleState.AddTransition(standingState, () => _inputHandler.CrouchTrigger.IsTriggered && _playerBodyController.CanStand(), _inputHandler.CrouchTrigger.ReleaseTrigger);
            crouchIdleState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);

            jumpState.AddTransition(runState, () => jumpState.CanGround() && _playerGravity.IsGrounded && _inputHandler.MoveForward && _inputHandler.SprintTriggered);
            jumpState.AddTransition(walkState, () => jumpState.CanGround() && _playerGravity.IsGrounded);
            jumpState.AddTransition(fallState, jumpState.InHightPoint);
            jumpState.AddTransition(grabLedgeState, () => _ledgeController.CanGrabToLedgeGrabPointInView());

            fallState.AddTransition(walkState, () => _playerGravity.IsGrounded);
            fallState.AddTransition(grabLedgeState, () => _ledgeController.CanGrabToLedgeGrabPointInView());

            grabLedgeState.AddTransition(fallState, () => _inputHandler.MoveBack && grabLedgeState.CanFinish());
            grabLedgeState.AddTransition(climbingUpState, () => _inputHandler.MoveForward && grabLedgeState.CanFinish() && climbingUpState.CanClimb());
            grabLedgeState.AddTransition(ledgeJumpState, () => _inputHandler.JumpTriggered && grabLedgeState.CanFinish());

            ledgeJumpState.AddTransition(grabLedgeState, () => _ledgeController.CanGrabToLedgeGrabPointInView());
            ledgeJumpState.AddTransition(fallState, ledgeJumpState.InHightPoint);

            climbingUpState.AddTransition(walkState, () => climbingUpState.ClimbFinish());
            climbingOnState.AddTransition(walkState, () => climbingOnState.ClimbFinish());
            climbingOverState.AddTransition(fallState, () => climbingOverState.ClimbFinish());

            _behaviorStateMachine = new BehaviorStateMachine(walkState, forcedStates);

        }

        private IDamageService CreateDamageService()
        {
            var damageService = new DamageService();

            damageService.Register(new FlatDamageProvider(_health));

            return damageService;
        }

        private void OnTakeDamage(IDamage damage)
        {
            _damageService.Provide(damage);
        }

        void Update()
        {
            _ledgeController.HandleFindingLedge();
            _playerRotationController.HandlePlayerRotation();
            _playerGravity.HandleGravity();
            _behaviorStateMachine.Update();
            _stepController.Update();
            if (_health.IsDead)
            {
                _behaviorStateMachine.ForseChangeState<DeathState>();
            }
        }

        void FixedUpdate()
        {
            _movementController.HandleMovement();
            _playerGravity.CheckGrounded();
        }
    }
}