using UnityEngine;

/// <summary>
/// Стандартное поведение противника пока он не обнаружил игрока
/// </summary>
public class DefaultState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;
    public bool IsPatroling()
    {
        return true;
    }
    public DefaultState(Sensor[] sensors, Transform transform )
    {
        _sensors = sensors;
        _transform = transform;
    }

    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter DefaultState");
    }
    
}