using UnityEngine;

public class VisualSensor : Sensor
{
    public bool SeenPlayer => IsActivated() && Target != null;

    [SerializeField] private float _viewAngle = 45f;
    [SerializeField] private LayerMask _ignoreLayer;
    [SerializeField] private Transform _eyePosition;
    public Transform Target;
    [SerializeField] private Transform[] _targets; // SensorPoints: грудь, голова, ноги и т.п.
    [SerializeField] private int _chestIndex = 0; // Индекс груди в массиве

    private Vector3 EyePosition => _eyePosition != null ? _eyePosition.position : transform.position + Vector3.up * 0.5f;

    private Ray _rayToPlayer;
    private Vector3 _attackRaySize;
    private float _rayWidth = 0.15f;
    private float _rayHeight = 0.2f;
    private float _nearDistance;

    public override Vector3? SignalSourcePosition => IsActivated() ? GetBestVisiblePointWithChestOverride()?.position : null;

    public Transform GetBestVisiblePointWithChestOverride()
    {
        if (_targets == null || _targets.Length == 0)
            return null;

        // 1. Сначала — грудь
        if (_chestIndex >= 0 && _chestIndex < _targets.Length)
        {
            Transform chest = _targets[_chestIndex];
            if (IsPointVisible(chest))
                return chest;
        }

        // 2. Если грудь не видно — ищем любую другую
        foreach (var point in _targets)
        {
            if (IsPointVisible(point))
                return point;
        }

        return null;
    }

    private bool IsPointVisible(Transform point)
    {
        Vector3 dirToPoint = point.position - EyePosition;
        float angle = Vector3.Angle(transform.forward, dirToPoint);
        if (angle > _viewAngle)
            return false;

        Ray ray = new Ray(EyePosition, dirToPoint.normalized);
        if (Physics.Raycast(ray, out var hit, Distance, ~_ignoreLayer))
        {
            return hit.transform == Target || hit.transform.IsChildOf(Target);
        }
        return false;
    }

    protected override float SignalInFieldOfView()
    {
        _nearDistance = Distance + Distance / 3f;

        if (_targets == null || _targets.Length == 0)
            return 0f;

        Transform bestPoint = GetBestVisiblePointWithChestOverride();
        if (bestPoint == null)
            return 0f;

        Vector3 toTarget = bestPoint.position - EyePosition;
        float distance = toTarget.magnitude;
        if (distance > Distance)
            return 0f;

        Vector3 direction = toTarget.normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > _viewAngle)
            return 0f;

        _rayToPlayer = new Ray(EyePosition, direction);
        if (Physics.Raycast(_rayToPlayer, out var hit, Distance, ~_ignoreLayer))
        {
            if (hit.transform == Target || hit.transform.IsChildOf(Target))
            {
                if (distance <= _nearDistance)
                    return 1f;

                float t = Mathf.InverseLerp(Distance, _nearDistance, distance);
                return Mathf.Lerp(0.2f, 1f, 1f - t);
            }
        }

        return 0;
    }

    public override bool SignalInAttackRay(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - EyePosition).normalized;
        _attackRaySize = new Vector3(_rayWidth / 2, _rayHeight / 2, 1);

        if (Physics.BoxCast(EyePosition, _attackRaySize, direction, out var hit, Quaternion.LookRotation(direction), Mathf.Infinity, ~_ignoreLayer))
        {
            if (hit.transform == Target || hit.transform.IsChildOf(Target))
            {
                return true;
            }
        }
        return false;
    }

    public void SetAngle(float angle)
    {
        _viewAngle = angle;
    }

    private void OnDrawGizmosSelected()
    {
        if (Target == null) return;

        Vector3 origin = EyePosition;

        Gizmos.color = Color.cyan;
        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle, 0) * transform.forward;

        Gizmos.DrawRay(origin, leftBoundary * Distance);
        Gizmos.DrawRay(origin, rightBoundary * Distance);

        Color nearColor = new Color(0f, 1f, 0f, 1f);
        Gizmos.color = nearColor;
        Gizmos.DrawWireSphere(origin, _nearDistance);
        Gizmos.DrawSphere(origin + transform.forward * _nearDistance, 0.15f);

        Color farColor = new Color(1f, 0f, 0f, 1f);
        Gizmos.color = farColor;
        Gizmos.DrawWireSphere(origin, Distance);

        Gizmos.color = Color.magenta;
        Vector3 ray1 = origin;
        ray1.x -= _rayWidth / 2;
        ray1.y -= _rayHeight / 2;
        Vector3 ray2 = origin;
        ray2.x -= _rayWidth / 2;
        ray2.y += _rayHeight / 2;
        Vector3 ray3 = origin;
        ray3.x += _rayWidth / 2;
        ray3.y -= _rayHeight / 2;
        Vector3 ray4 = origin;
        ray4.x += _rayWidth / 2;
        ray4.y += _rayHeight / 2;
        Gizmos.DrawRay(ray1, _eyePosition.forward * Distance);
        Gizmos.DrawRay(ray2, _eyePosition.forward * Distance);
        Gizmos.DrawRay(ray3, _eyePosition.forward * Distance);
        Gizmos.DrawRay(ray4, _eyePosition.forward * Distance);

        if (Application.isPlaying)
        {
            Gizmos.color = SignalStrength > 0 ? Color.green : Color.yellow;
            Gizmos.DrawRay(_rayToPlayer.origin, _rayToPlayer.direction * Distance);
        }
    }
}