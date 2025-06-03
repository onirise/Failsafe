using Failsafe.PlayerMovements;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

public class CrouchIdle : BehaviorState
{
    private readonly Transform _camera;
    private readonly PlayerMovementController _movementController;
    private readonly Vector3 _cameraOriginalPosition;
    private readonly PlayerMovementParameters _movementParametrs;
    private readonly PlayerNoiseController _playerNoiseController;
    private readonly StepController _stepController;
    public CrouchIdle(Transform camera, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController, StepController stepController)
    {
        _camera = camera;
        _movementController = movementController;
        _movementParametrs = movementParametrs;
        _playerNoiseController = playerNoiseController;
        _cameraOriginalPosition = camera.localPosition;
        _stepController = stepController;

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter " + nameof(CrouchIdle));
        _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.CrouchHeight));
        _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Minimum);
        _stepController.Disable();
        _movementController.Move(Vector3.zero);
    }

    public override void Exit()
    {
        _camera.localPosition = _cameraOriginalPosition;

    }
}
