using UnityEngine;
/// <summary>
/// Реализация визуального обнаружения игрока
/// </summary>
public class VisualSensor : Sensor
{
    public bool SeenPlayer => IsActivated() && Target != null;  
    [SerializeField]
    private float _viewAngle;
    /// <summary>
    /// Слой который игнорировать при рейкасте
    /// Должен быть слой врага чтобы он не попадал сам в себя
    /// </summary>
    [SerializeField]
    private LayerMask _ignoreLayer;
    /// <summary>
    /// Смещение луча, чтобы он шел из глаз врага 
    /// </summary>
    private Vector3 _rayOffset = Vector3.up * 0.5f;
    [SerializeField] private Transform _eyePosition;
    private Vector3 EyePosition => _eyePosition != null ? _eyePosition.position : transform.position + _rayOffset;

    /// <summary>
    /// Размеры атакующего луча, идущего из глаз врага
    /// </summary>
    private float _rayWidth = 0.15f;
    private float _rayHeight = 0.2f;

    /// <summary>
    /// Объект, который нужно обнаружить, в данном случае игрок
    /// </summary>
    public Transform Target;
    public override Vector3? SignalSourcePosition => IsActivated() ? Target.position : null;

    private Ray _rayToPlayer;

    private Vector3 _attackRaySize;

    float _nearDistance;
    protected override float SignalInFieldOfView()
    {
        _nearDistance = Distance + Distance / 3f;

        if (Target == null)
            return 0f;

        Vector3 toTarget = Target.position - EyePosition;
        float distance = toTarget.magnitude;
        if (distance > Distance)
            return 0f;

        Vector3 direction = toTarget.normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle > _viewAngle)
            return 0f;

        // Это можно реализовать интереснее, делать рейкаст в каждую часть тела игрока (руку, ногу, голову, туловище)
        // Сколько частей тела замечено, такая сила сигнала будет выведена
        // Пока используется одна капсула, поэтому значение всегда 1
        _rayToPlayer = new Ray(EyePosition, direction);
        if (Physics.Raycast(_rayToPlayer, out var hit, Distance, ~_ignoreLayer))
        {
            // сейчас обнаружение игрока идет по тэгу Player
            if (hit.transform == Target || hit.transform.IsChildOf(Target))
            {
                // Ближняя зона: мгновенное обнаружение
                if (distance <= _nearDistance)
                {
                    return 1f;
                }
                else
                {
                    // Дальняя зона: ослабевающий сигнал
                    float t = Mathf.InverseLerp(Distance, _nearDistance, distance); // от 1 → 0
                    return Mathf.Lerp(0.2f, 1f, 1f - t); // сигнал 0.2–1.0
                }
            }

        }
        return 0;
    }

    public override bool SignalInAttackRay(Vector3 targetPosition)
    {
        Vector3 direction = _eyePosition.forward;
        _attackRaySize = new Vector3(_rayWidth / 2, _rayHeight / 2, 1);

        if (Physics.BoxCast(EyePosition, _attackRaySize, direction, out var hit, transform.rotation, Mathf.Infinity, ~_ignoreLayer))
        {
            // сейчас обнаружение игрока идет по тэгу Player
            if (hit.transform == Target || hit.transform.IsChildOf(Target))
            {
                return true;
            }

        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (Target == null) return;

        Vector3 origin = EyePosition;

        // === Углы поля зрения ===
        Gizmos.color = Color.cyan;
        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle, 0) * transform.forward;

        Gizmos.DrawRay(origin, leftBoundary * Distance);
        Gizmos.DrawRay(origin, rightBoundary * Distance);

        // === Ближняя зона (ярко-зелёная, толстая) ===
        Color nearColor = new Color(0f, 1f, 0f, 1f); // насыщенный зелёный
        Gizmos.color = nearColor;
        Gizmos.DrawWireSphere(origin, _nearDistance);

        // Центральная точка ближней зоны
        Gizmos.DrawSphere(origin + transform.forward * _nearDistance, 0.15f);

        // === Дальняя зона (ярко-красная) ===
        Color farColor = new Color(1f, 0f, 0f, 1f); // насыщенный красный
        Gizmos.color = farColor;
        Gizmos.DrawWireSphere(origin, Distance);

        // === Луч атаки (фиолетовый) ===
        Color rayColor = Color.magenta; // насыщенный фиолетовый

        Gizmos.color = rayColor;
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

        // === Визуализация луча до цели (при игре) ===
        if (Application.isPlaying)
        {
            Gizmos.color = SignalStrength > 0 ? Color.green : Color.yellow;
            Gizmos.DrawRay(_rayToPlayer.origin, _rayToPlayer.direction * Distance);
        }
    }
}
