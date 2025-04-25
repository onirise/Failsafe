using UnityEngine;

/// <summary>
/// Стандартное поведение противника пока он не обнаружил игрока
/// </summary>
public class DefaultState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;

    public DefaultState(Sensor[] sensors, Transform transform)
    {
        _sensors = sensors;
        _transform = transform;
    }

    private float warningProgres;
    private float warningTime = 1;

    public bool PlayerSpoted() => warningProgres >= warningTime;

    public override void Enter()
    {
        base.Enter();
        warningProgres = 0;
        Debug.Log("Enter DefaultState");
    }

    public override void Update()
    {
        foreach (var sensor in _sensors)
        {
            if (sensor.IsActivated())
            {
                warningProgres += sensor.SignalStrength * Time.deltaTime;
            }
        }
    }
}