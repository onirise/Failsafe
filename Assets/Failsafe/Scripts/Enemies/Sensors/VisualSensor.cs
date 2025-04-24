using UnityEngine;

/// <summary>
/// Реализация визуального обнаружения игрока
/// </summary>
public class VisualSensor : Sensor
{
    [SerializeField]
    private float _viewRange;
    [SerializeField]
    private float _viewWidth;
    /// <summary>
    /// Объект, который нужно обнаружить, в данном случае игрок
    /// </summary>
    [SerializeField]
    private Transform _target;
    public override Vector3? SignalSourcePosition => IsActivated() ? _target.position : null;

    protected override float SignalInFieldOfView()
    {
        var distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget > _viewRange) return 0;

        var viewDirection = transform.forward;
        var targetDirection = Vector3.Normalize(_target.position - transform.position);

        var distanceToViewPoint = Vector3.Distance(viewDirection, targetDirection);
        if (distanceToViewPoint > _viewWidth) return 0;

        // Это можно реализовать интереснее, делать рейкаст в каждую часть тела игрока (руку, ногу, голову, туловище)
        // Сколько частей тела замечено, такая сила сигнала будет выведена
        // Пока используется одна капсула, поэтому значение всегда 1
        if (Physics.Raycast(transform.position, targetDirection, out var hit, _viewRange))
        {
            // сейчас обнаружение игрока идет по тэгу Player
            if (hit.transform.tag == _target.tag)
            {
                return 1;
            }
        }
        return 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);

        if (_target == null) return;
        Gizmos.color = SignalStrength > 0 ? Color.green : Color.yellow;
        var targetDiraction = Vector3.Normalize(_target.position - transform.position);
        Gizmos.DrawRay(transform.position, targetDiraction);
    }
}