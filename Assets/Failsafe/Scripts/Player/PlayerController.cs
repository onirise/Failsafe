using UnityEngine;
using PlayerStates;
using UnityEngine.InputSystem;

/// <summary>
/// Контроллер персонажа
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement params")]
    [SerializeReference] private PlayerMovementParametrs _movementParametrs = new PlayerMovementParametrs();
    [SerializeField] private InputActionAsset _inputActionAsset;

    private Transform _playerCamera;
    private CharacterController _characterController;
    private BehaviorStateMachine _behaviorStateMachine;
    private InputHandler _inputHandler;

    float _cameraYRotation = 0f;
    [SerializeField] float _coyoteTime = 0.1f;
    float _coyoteTimeProgress = 0f;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = transform.Find("Camera");
        _inputHandler = new InputHandler(_inputActionAsset);
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var standingState = new StandingState(_inputHandler);
        var walkState = new WalkState(_inputHandler, _characterController, _movementParametrs);
        var runState = new SprintState(_inputHandler, _characterController, _movementParametrs);
        var crouchState = new CrouchState(_inputHandler, _characterController, _movementParametrs, _playerCamera);
        var jumpState = new JumpState(_inputHandler, _characterController, _movementParametrs);
        var fallState = new FallState(_inputHandler, _characterController, _movementParametrs);
        var slideState = new SlideState(_inputHandler, _characterController, _movementParametrs, _playerCamera);

        walkState.AddTransition(runState, () => _inputHandler.MoveForward && _inputHandler.SprintTriggered);
        walkState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
        walkState.AddTransition(crouchState, () => _inputHandler.CrouchTriggered);
        walkState.AddTransition(fallState, () => IsFalling());

        runState.AddTransition(walkState, () => !(_inputHandler.MoveForward && _inputHandler.SprintTriggered));
        runState.AddTransition(jumpState, () => _inputHandler.JumpTriggered);
        runState.AddTransition(slideState, () => _inputHandler.CrouchTriggered && runState.CanSlide());
        runState.AddTransition(fallState, () => IsFalling());

        //slideState.AddTransition(runState, () => _inputHandler.SprintTriggered && slideState.SlideFinished());
        slideState.AddTransition(crouchState, () => _inputHandler.CrouchTriggered && slideState.SlideFinished());
        slideState.AddTransition(walkState, () => (!_inputHandler.CrouchTriggered && slideState.CanStand()) || slideState.SlideFinished());
        slideState.AddTransition(fallState, () => IsFalling());

        crouchState.AddTransition(runState, () => _inputHandler.SprintTriggered && crouchState.CanStand());
        crouchState.AddTransition(walkState, () => !_inputHandler.CrouchTriggered && crouchState.CanStand());
        crouchState.AddTransition(fallState, () => IsFalling());

        jumpState.AddTransition(runState, () => IsGrounded() && _inputHandler.SprintTriggered);
        jumpState.AddTransition(walkState, () => IsGrounded());
        jumpState.AddTransition(fallState, jumpState.InHightPoint);

        fallState.AddTransition(walkState, () => IsGrounded());


        _behaviorStateMachine = new BehaviorStateMachine(walkState);
    }

    void Update()
    {
        HandlePlayerRotation();
        HandleGravity();
        _behaviorStateMachine.Update();
    }

    private float _mouseSensitivity = 50f;
    // TODO: Вынести в отдельный класс:
    // В некоторых состояниях может работать по разному (Скольжение, Зацепление)
    // В некоторых не должно работать (Взбирание)
    private void HandlePlayerRotation()
    {
        var rotation = _inputHandler.RotationInput * _mouseSensitivity * Time.deltaTime;
        _cameraYRotation += rotation.y;
        _cameraYRotation = Mathf.Clamp(_cameraYRotation, -90f, 90f);

        transform.Rotate(Vector3.up * rotation.x);
        _playerCamera.transform.localRotation = Quaternion.AngleAxis(_cameraYRotation, Vector3.left);
    }

    private void HandleGravity()
    {
        var gravity = new Vector3(0, -_movementParametrs.gravityForce, 0) * Time.deltaTime;
        _characterController.Move(gravity);
        if (_characterController.isGrounded)
        {
            _coyoteTimeProgress = 0;
        }
        else
        {
            _coyoteTimeProgress += Time.deltaTime;
        }
    }

    private bool IsGrounded() => _coyoteTimeProgress <= 0;
    private bool IsFalling() => _coyoteTimeProgress > _coyoteTime;
}
