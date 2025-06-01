using Failsafe.PlayerMovements;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

public class CrouchIdle : BehaviorState
{
    private readonly Transform _camera;
    private readonly Vector3 _cameraOriginalPosition;
    private readonly PlayerMovementParameters _movementParametrs;
    private readonly StepController _stepController;
    public CrouchIdle(Transform camera, PlayerMovementParameters movementParametrs, StepController stepController)
    {
        _camera = camera;
        _movementParametrs = movementParametrs;
        _cameraOriginalPosition = camera.localPosition;
        _stepController = stepController;

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter " + nameof(CrouchIdle));
        _camera.localPosition += Vector3.down * (_cameraOriginalPosition.y * (1 - _movementParametrs.CrouchHeight));
        _stepController.Disable();

    }

    public override void Exit()
    {
        _camera.localPosition = _cameraOriginalPosition;

    }
}
