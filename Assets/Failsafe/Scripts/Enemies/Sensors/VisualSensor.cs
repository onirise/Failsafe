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
    /// Слой который игнорировать при рейкасте
    /// Должен быть слой врага чтобы он не попадал сам в себя
    /// </summary>
    [SerializeField]
    private LayerMask _ignoreLayer;
    /// <summary>
    /// Смещение луча, чтобы он шел из глаз врага 
    /// </summary>
    private Vector3 _rayOffset = Vector3.up * 1.5f;
    /// <summary>
    /// Объект, который нужно обнаружить, в данном случае игрок
    /// </summary>
    public Transform _target;
    public override Vector3? SignalSourcePosition => IsActivated() ? _target.position : null;

    private Ray _rayToPlayer;

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
        _rayToPlayer = new Ray(transform.position + _rayOffset, targetDirection);
        if (Physics.Raycast(_rayToPlayer, out var hit, _viewRange, ~_ignoreLayer))
        {
            // сейчас обнаружение игрока идет по тэгу Player
            if (hit.transform.parent.tag == _target.tag)
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
        Gizmos.DrawRay(_rayToPlayer);
    }
}