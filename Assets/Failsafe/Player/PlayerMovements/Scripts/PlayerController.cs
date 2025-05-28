using Failsafe.Player.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;
using Failsafe.PlayerMovements.Controllers;
using Failsafe.PlayerMovements.States;

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

        private Transform _playerCamera;
        private Transform _playerGrabPoint;
        private CharacterController _characterController;
        private PlayerRotationController _playerRotationController;
        private BehaviorStateMachine _behaviorStateMachine;
        private InputHandler _inputHandler;
        private LedgeDetector _ledgeDetector;
        private PlayerGravityController _playerGravity;
        private PlayerNoiseController _noiseController;
        private PhysicsInteraction _physicsInteraction;

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _playerCamera = transform.Find("Camera");
            _playerGrabPoint = transform.Find("ObstacleGrabPoint");
            _inputHandler = new InputHandler(_inputActionAsset);
            _playerRotationController = new PlayerRotationController(transform, _playerCamera, _inputHandler);
            _ledgeDetector = new LedgeDetector(transform, _playerCamera, _playerGrabPoint);
            _playerGravity = new PlayerGravityController(_characterController, _movementParametrs);
            _noiseController = new PlayerNoiseController(transform, _noiseParametrs);
            _physicsInteraction = GetComponent<PhysicsInteraction>();
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            var standingState = new StandingState(_inputHandler);
            var walkState = new WalkState(_inputHandler, _characterController, _movementParametrs, _noiseController);
            var runState = new SprintState(_inputHandler, _characterController, _movementParametrs, _noiseController);
            var slideState = new SlideState(_inputHandler, _characterController, _movementParametrs, _playerCamera, _playerRotationController);
            var crouchState = new CrouchState(_inputHandler, _characterController, _movementParametrs, _playerCamera, _noiseController);
            var jumpState = new JumpState(_inputHandler, _characterController, _movementParametrs);
            var fallState = new FallState(_inputHandler, _characterController, _movementParametrs, _noiseController);
            var grabLedgeState = new GrabLedgeState(_inputHandler, _characterController, _movementParametrs, _playerGravity, _ledgeDetector, _playerRotationController, _playerGrabPoint);
            var climbingState = new ClimbingState(_inputHandler, _characterController, _movementParametrs, _playerGravity, _playerGrabPoint);
            var ledgeJumpState = new LedgeJumpState(_inputHandler, _characterController, _movementParametrs, _playerCamera);
            var grabOrDropState = new GrabOrDropState(_physicsInteraction);
            var throwState = new ThrowState(_physicsInteraction);

            walkState.AddTransition(runState, () => _inputHandler.MoveForward && _inputHandler.SprintTriggered);
            walkState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
            walkState.AddTransition(crouchState, () => _inputHandler.CrouchTriggered);
            walkState.AddTransition(fallState, () => _playerGravity.IsFalling);
            walkState.AddTransition(grabOrDropState, () => _inputHandler.GrabOrDropTriggered);
            walkState.AddTransition(throwState, () => _inputHandler.AttackTriggered && _physicsInteraction.IsDragging);

            runState.AddTransition(walkState, () => !(_inputHandler.MoveForward && _inputHandler.SprintTriggered));
            runState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
            runState.AddTransition(slideState, () => _inputHandler.CrouchTriggered && runState.CanSlide());
            runState.AddTransition(fallState, () => _playerGravity.IsFalling);
            runState.AddTransition(grabOrDropState, () => _inputHandler.GrabOrDropTriggered);
            runState.AddTransition(throwState, () => _inputHandler.AttackTriggered && _physicsInteraction.IsDragging);

            //slideState.AddTransition(runState, () => _inputHandler.SprintTriggered && slideState.SlideFinished());
            slideState.AddTransition(crouchState, () => _inputHandler.CrouchTriggered && slideState.SlideFinished());
            slideState.AddTransition(walkState, () => (!_inputHandler.CrouchTriggered && slideState.CanStand()) || slideState.SlideFinished());
            slideState.AddTransition(fallState, () => _playerGravity.IsFalling);
            slideState.AddTransition(grabOrDropState, () => _inputHandler.GrabOrDropTriggered);
            slideState.AddTransition(throwState, () => _inputHandler.AttackTriggered && _physicsInteraction.IsDragging);

            crouchState.AddTransition(runState, () => _inputHandler.MoveForward && _inputHandler.SprintTriggered && crouchState.CanStand());
            crouchState.AddTransition(walkState, () => !_inputHandler.CrouchTriggered && crouchState.CanStand());
            crouchState.AddTransition(fallState, () => _playerGravity.IsFalling);
            crouchState.AddTransition(grabOrDropState, () => _inputHandler.GrabOrDropTriggered);
            crouchState.AddTransition(throwState, () => _inputHandler.AttackTriggered && _physicsInteraction.IsDragging);

            jumpState.AddTransition(runState, () => _playerGravity.IsGrounded && _inputHandler.SprintTriggered);
            jumpState.AddTransition(walkState, () => _playerGravity.IsGrounded);
            jumpState.AddTransition(fallState, jumpState.InHightPoint);
            jumpState.AddTransition(grabLedgeState, () => { var ledge = _ledgeDetector.LedgeInView; return ledge.IsFound && ledge.InPlayerView && ledge.AroundGrabPoint; });
            jumpState.AddTransition(grabOrDropState, () => _inputHandler.GrabOrDropTriggered);
            jumpState.AddTransition(throwState, () => _inputHandler.AttackTriggered && _physicsInteraction.IsDragging);

            fallState.AddTransition(walkState, () => _playerGravity.IsGrounded);
            fallState.AddTransition(grabLedgeState, () => { var ledge = _ledgeDetector.LedgeInView; return ledge.IsFound && ledge.InPlayerView && ledge.AroundGrabPoint; });
            fallState.AddTransition(grabOrDropState, () => _inputHandler.GrabOrDropTriggered);
            fallState.AddTransition(throwState, () => _inputHandler.AttackTriggered && _physicsInteraction.IsDragging);
            
            grabLedgeState.AddTransition(fallState, () => _inputHandler.MoveBack && grabLedgeState.CanFinish());
            grabLedgeState.AddTransition(climbingState, () => _inputHandler.MoveForward && grabLedgeState.CanFinish() && climbingState.CanClimb());
            grabLedgeState.AddTransition(ledgeJumpState, () => _inputHandler.JumpTriggered && grabLedgeState.CanFinish());

            ledgeJumpState.AddTransition(grabLedgeState, () => { var ledge = _ledgeDetector.LedgeInView; return ledge.IsFound && ledge.InPlayerView && ledge.AroundGrabPoint; });
            ledgeJumpState.AddTransition(fallState, ledgeJumpState.InHightPoint);

            climbingState.AddTransition(walkState, () => climbingState.ClimbFinish());
            
            grabOrDropState.AddTransition(walkState, () => !_inputHandler.GrabOrDropTriggered);
            grabOrDropState.AddTransition(runState, () => !_inputHandler.GrabOrDropTriggered);
            grabOrDropState.AddTransition(slideState, () => !_inputHandler.GrabOrDropTriggered);
            grabOrDropState.AddTransition(crouchState, () => !_inputHandler.GrabOrDropTriggered);
            grabOrDropState.AddTransition(jumpState, () => !_inputHandler.GrabOrDropTriggered);
            grabOrDropState.AddTransition(fallState, () => !_inputHandler.GrabOrDropTriggered);
            
            throwState.AddTransition(walkState, () => !_inputHandler.GrabOrDropTriggered);
            throwState.AddTransition(runState, () => !_inputHandler.GrabOrDropTriggered);
            throwState.AddTransition(slideState, () => !_inputHandler.GrabOrDropTriggered);
            throwState.AddTransition(crouchState, () => !_inputHandler.GrabOrDropTriggered);
            throwState.AddTransition(jumpState, () => !_inputHandler.GrabOrDropTriggered);
            throwState.AddTransition(fallState, () => !_inputHandler.GrabOrDropTriggered);

            _behaviorStateMachine = new BehaviorStateMachine(walkState);
        }

        void Update()
        {
            _ledgeDetector.HandleFindingLedge();
            _playerRotationController.HandlePlayerRotation();
            _playerGravity.HandleGravity();
            _behaviorStateMachine.Update();
        }
    }
}