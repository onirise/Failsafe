using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Преследование объекта, попавшего в сенсор
/// Противник старается достигнуть точки, где он в последний раз заметил игрока
/// </summary>
public class ChasingState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;
    private Vector3? _chasingPosition;

    private NavMeshAgent _navMeshAgent;

    private float _loseTime = 1;
    private float _loseProgres;

    public ChasingState(Sensor[] sensors, Transform currentTransform, NavMeshAgent navMeshAgent)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _navMeshAgent = navMeshAgent;
    }

    public bool PlayerLost() => _loseProgres >= _loseTime;

    public bool IsChasingPointReached()
    {
        if (Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    public override void Enter()
    {
        base.Enter();
        _loseProgres = 0;
        Debug.Log("Enter ChasingState");
    }

    public override void Update()
    {
        bool anySensorIsActive = false;
        foreach (var sensor in _sensors)
        {
            if (sensor.IsActivated())
            {
                anySensorIsActive = true;
                _loseProgres = 0;
                _chasingPosition = sensor.SignalSourcePosition;
                break;
            }
        }
        if (IsChasingPointReached() && !anySensorIsActive)
        {
            _loseProgres += Time.deltaTime;
        }
        if (_chasingPosition == null)
        {
            return;
        }
        _navMeshAgent.SetDestination(_chasingPosition.Value);
        _transform.LookAt(_chasingPosition.Value, _transform.up);
    }
}