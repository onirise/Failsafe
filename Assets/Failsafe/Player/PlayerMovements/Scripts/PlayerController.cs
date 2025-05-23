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

            walkState.AddTransition(runState, () => _inputHandler.MoveForward && _inputHandler.SprintTriggered);
            walkState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
            walkState.AddTransition(crouchState, () => _inputHandler.CrouchTriggered);
            walkState.AddTransition(fallState, () => _playerGravity.IsFalling);

            runState.AddTransition(walkState, () => !(_inputHandler.MoveForward && _inputHandler.SprintTriggered));
            runState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
            runState.AddTransition(slideState, () => _inputHandler.CrouchTriggered && runState.CanSlide());
            runState.AddTransition(fallState, () => _playerGravity.IsFalling);

            //slideState.AddTransition(runState, () => _inputHandler.SprintTriggered && slideState.SlideFinished());
            slideState.AddTransition(crouchState, () => _inputHandler.CrouchTriggered && slideState.SlideFinished());
            slideState.AddTransition(walkState, () => (!_inputHandler.CrouchTriggered && slideState.CanStand()) || slideState.SlideFinished());
            slideState.AddTransition(fallState, () => _playerGravity.IsFalling);

            crouchState.AddTransition(runState, () => _inputHandler.SprintTriggered && crouchState.CanStand());
            crouchState.AddTransition(walkState, () => !_inputHandler.CrouchTriggered && crouchState.CanStand());
            crouchState.AddTransition(fallState, () => _playerGravity.IsFalling);

            jumpState.AddTransition(runState, () => _playerGravity.IsGrounded && _inputHandler.SprintTriggered);
            jumpState.AddTransition(walkState, () => _playerGravity.IsGrounded);
            jumpState.AddTransition(fallState, jumpState.InHightPoint);
            jumpState.AddTransition(grabLedgeState, () => { var ledge = _ledgeDetector.LedgeInView; return ledge.IsFound && ledge.InPlayerView && ledge.AroundGrabPoint; });

            fallState.AddTransition(walkState, () => _playerGravity.IsGrounded);
            fallState.AddTransition(grabLedgeState, () => { var ledge = _ledgeDetector.LedgeInView; return ledge.IsFound && ledge.InPlayerView && ledge.AroundGrabPoint; });

            grabLedgeState.AddTransition(fallState, () => _inputHandler.MoveBack && grabLedgeState.CanFinish());
            grabLedgeState.AddTransition(climbingState, () => _inputHandler.MoveForward && grabLedgeState.CanFinish() && climbingState.CanClimb());
            grabLedgeState.AddTransition(ledgeJumpState, () => _inputHandler.JumpTriggered && grabLedgeState.CanFinish());

            ledgeJumpState.AddTransition(grabLedgeState, () => { var ledge = _ledgeDetector.LedgeInView; return ledge.IsFound && ledge.InPlayerView && ledge.AroundGrabPoint; });
            ledgeJumpState.AddTransition(fallState, ledgeJumpState.InHightPoint);

            climbingState.AddTransition(walkState, () => climbingState.ClimbFinish());

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