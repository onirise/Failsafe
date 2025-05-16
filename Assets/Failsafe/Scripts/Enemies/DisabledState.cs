using UnityEngine;

/// <summary>
/// Состояние деактивации
/// </summary>
public class DisabledState : BehaviorForcedState
{
    private float _disableTime = 5f;
    private float _disableProgress;

    public override void Enter()
    {
        base.Enter();
        _disableProgress = 0;
        Debug.Log("Enter DisabledState");
    }

    public override void Update()
    {
        _disableProgress += Time.deltaTime;
    }

    public override BehaviorState DecideNextState()
    {
        if (_disableProgress < _disableTime) return this;
        return _previousState;
    }
}