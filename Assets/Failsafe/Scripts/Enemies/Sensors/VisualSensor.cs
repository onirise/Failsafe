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

    public override Transform ObjectThatActivatedSensor => IsActivated() ? _target : null;

    public override bool IsInFieldOfView()
    {
        var distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget > _viewRange) return false;

        var viewDirection = transform.forward;
        var targetDirection = Vector3.Normalize(_target.position - transform.position);

        var distanceToViewPoint = Vector3.Distance(viewDirection, targetDirection);
        if (distanceToViewPoint > _viewWidth) return false;

        if (Physics.Raycast(transform.position, targetDirection, out var hit, _viewRange))
        {
            // сейчас обнаружение игрока идет по тэгу Player
            if (hit.transform.tag == _target.tag)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = IsInFieldOfView() ? Color.green : Color.yellow;
        var targetDiraction = Vector3.Normalize(_target.position - transform.position);
        Gizmos.DrawRay(transform.position, targetDiraction);
    }
}