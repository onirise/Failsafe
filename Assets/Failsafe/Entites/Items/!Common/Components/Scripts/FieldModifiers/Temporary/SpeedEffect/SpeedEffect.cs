using UnityEngine;
using System.Collections;

public class SpeedEffect : MonoBehaviour
{
    public SpeedEffectData Data;

    void Start()
    {
        ChangeSpeed();
    }

    public IEnumerator ChangeSpeed()
    {
        // var movementParams = GetComponent<PlayerController>().MovementParametrs;
        // movementParams.walkSpeed = movementParams.crouchSpeed = movementParams.runSpeed *= Data.SpeedFactor;
        yield return new WaitForSeconds(Data.Duration);
        // movementParams.walkSpeed = movementParams.crouchSpeed = movementParams.runSpeed *= 1 / Data.SpeedFactor;
        // Destroy(this);
    }
}
