using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;
    [SerializeField] private float crouchMultiplier = 0.5f;

    [Header("Jump Parameters")]
    //[SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float minJumpHeight = 0.5f;    // Минимальная высота
    [SerializeField] private float maxJumpHeight = 2.0f;    // Максимальная высота
    [SerializeField] private float jumpRiseTime  = 0.2f;         // Время до макс. высоты

    //[SerializeField] private bool hasJumped = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool jumpKeyReleased  = true;

    

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f; // Высота камеры при приседании (половина от стандартной)
    [SerializeField] private float standingHeight = 1.8f; // Стандартная высота камеры
    [SerializeField] private float crouchSmoothTime = 0.2f; // Плавность опускания/поднятия

    [Header("Look Parameters")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;

    [Header ("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler playerInputHandler;

    [Header ("Statements")]
    [SerializeField] private bool isMoving;

    private float jumpProgress;
    private float initialJumpVelocity;
    private float targetHeight;
    private float currentHeightVelocity; // Для SmoothDamp
    private Vector3 cameraStandingPos; // Стандартная позиция камеры

    private Vector3 currentMovement;
    private bool _forceStandUp;
    private float verticalRotation;
    //private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 1);

    /*private float CurrentSpeed => walkSpeed * (playerInputHandler.SprintTriggered ? sprintMultiplier : 
                                              (playerInputHandler.CrouchTriggered ? crouchMultiplier : 1));
    */
    private float CurrentSpeed
{
    get
    {
        if (playerInputHandler.SprintTriggered)
            return walkSpeed * sprintMultiplier;
        if (playerInputHandler.CrouchTriggered)
            return walkSpeed * crouchMultiplier;
        return walkSpeed;
    }
}

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
{
    
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            UnityEngine.Debug.LogWarning("MainCamera не назначена в инспекторе! Используется Camera.main.");
        }

        cameraStandingPos = mainCamera.transform.localPosition; // Запоминаем дефолтную позицию
        targetHeight = standingHeight;
}

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCrouching();
    }


    private void HandleCrouching()
{
    // Определяем целевую высоту (присели или нет)
    if (playerInputHandler.SprintTriggered && playerInputHandler.CrouchTriggered)
    {
        _forceStandUp = true;
    }

    targetHeight = _forceStandUp ? standingHeight : 
                 (playerInputHandler.CrouchTriggered ? crouchHeight : standingHeight);

    // Сброс флага, когда достигли стоячей позиции
    if (_forceStandUp && Mathf.Approximately(mainCamera.transform.localPosition.y, standingHeight))
    {
        _forceStandUp = false;
    }

    // Плавно меняем высоту камеры
    float newHeight = Mathf.SmoothDamp(
        mainCamera.transform.localPosition.y,
        targetHeight,
        ref currentHeightVelocity,
        crouchSmoothTime
    );

    // Применяем новую позицию камеры
    mainCamera.transform.localPosition = new Vector3(
        cameraStandingPos.x,
        newHeight,
        cameraStandingPos.z
    );
}

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

private void HandleJumping()
{
    if (characterController.isGrounded)
    {
        currentMovement.y = -0.5f;
        
        // Защита от повторного прыжка: прыгаем ТОЛЬКО при новом нажатии
        if (playerInputHandler.JumpTriggered && !playerInputHandler.CrouchTriggered && jumpKeyReleased)
        {
            isJumping = true;
            jumpProgress = 0f;
            jumpKeyReleased = false; // Блокируем повторный прыжок
            initialJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * minJumpHeight);
            currentMovement.y = initialJumpVelocity;
        }
        
        // Сбрасываем флаг, когда клавиша отпущена
        if (!playerInputHandler.JumpTriggered)
        {
            jumpKeyReleased = true;
        }
    }
    else
    {
        // Плавный рост прыжка при удержании
        if (isJumping && playerInputHandler.JumpTriggered && jumpProgress < jumpRiseTime)
        {
            jumpProgress += Time.deltaTime;
            float addedForce = Mathf.Lerp(0, maxJumpHeight - minJumpHeight, jumpProgress / jumpRiseTime);
            currentMovement.y = initialJumpVelocity + addedForce;
        }
        
        // Прерывание прыжка
        if (!playerInputHandler.JumpTriggered)
        {
            isJumping = false;
        }

        // Усиленная гравитация
        currentMovement.y += Physics.gravity.y * gravityScale * Time.deltaTime;
    }
}

    /*private void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (playerInputHandler.JumpTriggered && !hasJumped && !playerInputHandler.CrouchTriggered)
            {
                isJumping = true;
                jumpStartTime = Time.time;
                initialJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * minJumpHeight);
                currentMovement.y = initialJumpVelocity;
                //hasJumped = true;
            }

            if (!playerInputHandler.JumpTriggered)
        {
            hasJumped = false;
        }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }*/

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        isMoving = characterController.isGrounded && 
               new Vector3(currentMovement.x, 0, currentMovement.z).sqrMagnitude > 0.1f;

        HandleJumping();
        //DebugMovement();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    [System.Diagnostics.Conditional("DEBUG")]
    /*private void DebugMovement() {
    UnityEngine.Debug.Log($"Current speed: {CurrentSpeed}");
    }*/

    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);
    }

    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalRotation = Mathf.Clamp(verticalRotation - rotationAmount, -upDownLookRange, upDownLookRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleRotation()
    {
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }
}
