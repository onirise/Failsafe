using Cysharp.Threading.Tasks;
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

    private float warningProgress;
    private float warningTime = 1;


    public bool PlayerSpotted() => warningProgress >= warningTime;

    public override void Enter()
    {
        base.Enter();
        warningProgress = 0;
        Debug.Log("Enter DefaultState");
    }

    public override void Update()
    {
        foreach (var sensor in _sensors)
        {
            if (sensor.IsActivated())
            {
                warningProgress += sensor.SignalStrength * Time.deltaTime;
            }
        }
    }
}