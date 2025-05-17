using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс работы с инпутом от игрока
/// </summary>
public class InputHandler
{
    private InputActionAsset playerControls;

    public InputHandler(InputActionAsset playerControls)
    {
        this.playerControls = playerControls;
        Init();
    }

    private string actionMapName = "Player";
    private string movement = "Movement";
    private string rotation = "Rotation";
    private string jump = "Jump";
    private string sprint = "Sprint";
    private string crouch = "Crouch";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;

    public Vector2 MovementInput { get; private set; }
    public bool MoveForward => MovementInput.y > 0;
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
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        crouchAction = mapReference.FindAction(crouch);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        crouchAction.performed += inputInfo => CrouchTriggered = true;
        crouchAction.canceled += inputInfo => CrouchTriggered = false;
    }
}
