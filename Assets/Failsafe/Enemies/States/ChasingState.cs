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
    private Enemy_ScriptableObject _enemyConfig;
    private EnemyAnimator _enemyAnimator;
    private EnemyNavMeshActions _enemyNavMeshActions;
    private EnemyMemory _enemyMemory;
    private float _loseProgress;

    private float _distanceToPlayer;
    private bool _playerInSight;

    public ChasingState(Sensor[] sensors, Transform currentTransform, EnemyNavMeshActions enemyNavMeshActions, EnemyMemory enemyMemory, NavMeshAgent navMeshAgent, Enemy_ScriptableObject enemyConfig, EnemyAnimator enemyAnimator)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyNavMeshActions = enemyNavMeshActions;
        _navMeshAgent = navMeshAgent;   
        _enemyConfig = enemyConfig;
        _enemyAnimator = enemyAnimator;
        _enemyMemory = enemyMemory;
    }

    public bool PlayerInAttackRange() => _playerInSight && (_distanceToPlayer < _enemyConfig.AttackRangeMin);



    public override void Enter()
    {
        base.Enter();
        _loseProgress = 0;
        _playerInSight = false;
        _navMeshAgent.stoppingDistance = _enemyConfig.AttackRangeMin;
        _enemyAnimator.StartMove(_enemyConfig.ChaseSpeed);
        Debug.Log("Enter ChasingState");
    }

    public override void Update()
    {
        
        bool anySensorIsActive = false;
        foreach (var sensor in _sensors)
        {
            if (sensor is VisualSensor)
                if (sensor.IsActivated())
                {
                    _distanceToPlayer = ((Vector3)sensor.SignalSourcePosition - _transform.position).magnitude;
                    _playerInSight = true;
                    
                }
                else
                {
                    _playerInSight = false;
                }

            if (sensor.IsActivated())
            {
                anySensorIsActive = true;
                _loseProgress = 0;
                _enemyNavMeshActions.RotateToPoint((Vector3)sensor.SignalSourcePosition, 5f);
                _chasingPosition = sensor.SignalSourcePosition;
                _enemyMemory.SetLastKnownPlayerPosition(
                    sensor.SignalSourcePosition.Value,
                    (sensor.SignalSourcePosition.Value - _transform.position).normalized
                );                break;
            }
        }
       
        if (_chasingPosition == null)
        {
            return;
        }
        _enemyNavMeshActions.RunToPoint(_chasingPosition.Value, _enemyConfig.ChaseSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}