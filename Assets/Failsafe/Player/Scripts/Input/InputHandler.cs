using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс работы с инпутом от игрока
/// </summary>
public class InputHandler
{
    private readonly InputActionAsset _playerControls;

    public InputHandler(InputActionAsset playerControls)
    {
        _playerControls = playerControls;
        Init();
        SubscribeOnActionsPerformed();
    }

    ~InputHandler()
    {
        UnsubscribeOnActionsPerformed();
    }

    private const string _actionMapName = "Player";
    private const string _movement = "Movement";
    private const string _rotation = "Rotation";
    private const string _jump = "Jump";
    private const string _sprint = "Sprint";
    private const string _crouch = "Crouch";
    private const string _grabOrDrop = "GrabOrDrop";
    private const string _attack = "Attack";
    private const string _grabLedge = "GrabLedge";
    private const string _zoom = "Zoom";
    private const string _use = "Use";

    private InputAction _movementAction;
    private InputAction _rotationAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _crouchAction;
    public InputAction GrabOrDropAction;
    private InputAction _attackAction;
    private InputAction _grabLedgeAction;
    private InputAction _zoomAction;
    private InputAction _useAction;

    public List<InputAction> PerformedActions = new List<InputAction>();

    public Vector2 MovementInput { get; private set; }
    public bool MoveForward => MovementInput.y > 0;
    public bool MoveBack => MovementInput.y < 0;
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public InputTrigger CrouchTrigger { get; private set; } = new InputTrigger();
    public bool AttackTriggered { get; private set; }
    public InputTrigger GrabLedgeTrigger { get; private set; } = new InputTrigger();
    public bool ZoomTriggered { get; private set; }
    public InputTrigger UseTrigger { get; private set; } = new InputTrigger();


    /// <summary>
    /// Преобразовать MovementInput к нужному Transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public Vector3 GetRelativeMovement(Transform transform)
    {
        return Vector3.ClampMagnitude(MovementInput.x * transform.right + MovementInput.y * transform.forward, 1);
    }

    private void Init()
    {
        InputActionMap mapReference = _playerControls.FindActionMap(_actionMapName);

        _movementAction = mapReference.FindAction(_movement);
        _rotationAction = mapReference.FindAction(_rotation);
        _jumpAction = mapReference.FindAction(_jump);
        _sprintAction = mapReference.FindAction(_sprint);
        _crouchAction = mapReference.FindAction(_crouch);
        GrabOrDropAction = mapReference.FindAction(_grabOrDrop);
        _attackAction = mapReference.FindAction(_attack);
        _grabLedgeAction = mapReference.FindAction(_grabLedge);
        _zoomAction = mapReference.FindAction(_zoom);
        _useAction = mapReference.FindAction(_use);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeOnActionsPerformed()
    {
        foreach (var actionMap in _playerControls.actionMaps)
            foreach (var action in actionMap.actions)
                action.performed += AddPerformedAction;
    }

    private void UnsubscribeOnActionsPerformed()
    {
        foreach (var actionMap in _playerControls.actionMaps)
            foreach (var action in actionMap.actions)
                action.canceled -= RemovePerformedAction;
    }

    public void AddPerformedAction(InputAction.CallbackContext context)
    { if (!PerformedActions.Contains(context.action)) PerformedActions.Add(context.action); }

    public void RemovePerformedAction(InputAction.CallbackContext context) =>
        PerformedActions.Remove(context.action);


    private void SubscribeActionValuesToInputEvents()
    {
        _movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        _movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        _rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        _rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        _jumpAction.performed += inputInfo => JumpTriggered = true;
        _jumpAction.canceled += inputInfo => JumpTriggered = false;

        _sprintAction.performed += inputInfo => SprintTriggered = true;
        _sprintAction.canceled += inputInfo => SprintTriggered = false;

        _crouchAction.performed += CrouchTrigger.OnInputStart;
        _crouchAction.canceled += CrouchTrigger.OnInputCancel;

        _attackAction.performed += inputInfo => AttackTriggered = true;
        _attackAction.canceled += inputInfo => AttackTriggered = false;

        _grabLedgeAction.performed += GrabLedgeTrigger.OnInputStart;
        _grabLedgeAction.canceled += GrabLedgeTrigger.OnInputCancel;

        _zoomAction.performed += inputInfo => ZoomTriggered = true;
        _zoomAction.canceled += inputInfo => ZoomTriggered = false;

        _useAction.performed += UseTrigger.OnInputStart;
        _useAction.canceled += UseTrigger.OnInputCancel;

    }

    public class InputTrigger
    {
        /// <summary>
        /// Инпут активирован
        /// </summary>
        public bool IsTriggered { get; private set; }
        /// <summary>
        /// Инпут удерживается
        /// </summary>
        public bool IsPressed { get; private set; }

        public void OnInputStart(InputAction.CallbackContext context)
        {
            IsTriggered = true;
            IsPressed = true;
        }

        public void OnInputCancel(InputAction.CallbackContext context)
        {
            IsTriggered = false;
            IsPressed = false;
        }

        /// <summary>
        /// Вызывать когда инпут обработан.
        /// </summary>
        public void ReleaseTrigger()
        {
            IsTriggered = false;
        }
    }
}

