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
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusFar);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusNear);

        Vector3 viewAngleSprint01 = DirectionFromAngle(fov.transform.eulerAngles.y, - fov.angleFar / 2);
        Vector3 viewAngleSprint02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleFar / 2);
        Vector3 viewAngleNear01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angleNear / 2);
        Vector3 viewAngleNear02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angleNear / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleSprint01 * fov.radiusFar);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleSprint02 * fov.radiusFar);

        Handles.color = Color.blue;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleNear01 * fov.radiusNear);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleNear02 * fov.radiusNear);
        if (fov.canSeePlayerFar || fov.canSeePlayerNear)
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