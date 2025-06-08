using Failsafe.PlayerMovements;
using Failsafe.PlayerMovements.Controllers;
using UnityEngine;

public class CrouchIdle : BehaviorState
{
    private readonly PlayerBodyController _playerBodyController;
    private readonly PlayerMovementController _movementController;
    private readonly Vector3 _cameraOriginalPosition;
    private readonly PlayerMovementParameters _movementParametrs;
    private readonly PlayerNoiseController _playerNoiseController;
    private readonly StepController _stepController;
    public CrouchIdle(PlayerBodyController playerBodyController, PlayerMovementController movementController, PlayerMovementParameters movementParametrs, PlayerNoiseController playerNoiseController, StepController stepController)
    {
        _playerBodyController = playerBodyController;
        _movementController = movementController;
        _movementParametrs = movementParametrs;
        _playerNoiseController = playerNoiseController;
        _stepController = stepController;

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter " + nameof(CrouchIdle));
        _playerBodyController.Crouch();
        _playerNoiseController.SetNoiseStrength(PlayerNoiseVolume.Minimum);
        _stepController.Disable();
        _movementController.Move(Vector3.zero);
    }

    public override void Exit()
    {
        _playerBodyController.Stand();
    }
}
