using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
[CanEditMultipleObjects]

public class FieldofVievEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusWalking);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusSprinting);

        Vector3 viewAngleSprint01 = DirectionFromAngle(fov.transform.eulerAngles.y, - fov.angleSprint / 2);
        Vector3 viewAngleSprint02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleSprint / 2);
        Vector3 viewAngleWalk01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angleWalk / 2);
        Vector3 viewAngleWalk02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleWalk / 2);
        Vector3 viewAngleNear01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angleNear / 2);
        Vector3 viewAngleNear02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleNear / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleSprint01 * fov.radiusSprinting);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleSprint02 * fov.radiusSprinting);

        Handles.color = Color.red;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleWalk01 * fov.radiusWalking);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleWalk02 * fov.radiusWalking);

        Handles.color = Color.blue;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleNear01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleNear02 * fov.radius);
        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);

        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
