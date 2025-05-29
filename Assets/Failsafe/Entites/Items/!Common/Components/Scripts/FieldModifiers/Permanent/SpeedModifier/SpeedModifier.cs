using Failsafe.PlayerMovements;
using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    public SpeedModifierData Data;

    public void ChangeSpeed()
    {
        var pc = GetComponentInParent<PlayerController>();
        if (pc == null)
        {
            Debug.LogError("No PlayerController in parents");
            return;
        }

        // var movementParams = pc.MovementParametrs;
        // movementParams.walkSpeed *= Data.SpeedFactor;
        // movementParams.runSpeed *= Data.SpeedFactor;
        // movementParams.crouchSpeed *= Data.SpeedFactor;
    }
}
