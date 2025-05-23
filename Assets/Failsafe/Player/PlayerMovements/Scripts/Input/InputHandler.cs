using System.Collections;
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
    }

    private const string _actionMapName = "Player";
    private const string _movement = "Movement";
    private const string _rotation = "Rotation";
    private const string _jump = "Jump";
    private const string _sprint = "Sprint";
    private const string _crouch = "Crouch";

    private InputAction _movementAction;
    private InputAction _rotationAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    private InputAction _crouchAction;

    public Vector2 MovementInput { get; private set; }
    public bool MoveForward => MovementInput.y > 0;
    public bool MoveBack => MovementInput.y < 0;
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool CrouchTriggered { get; private set; }

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

        SubscribeActionValuesToInputEvents();
    }

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

        _crouchAction.performed += inputInfo => CrouchTriggered = true;
        _crouchAction.canceled += inputInfo => CrouchTriggered = false;
    }
}