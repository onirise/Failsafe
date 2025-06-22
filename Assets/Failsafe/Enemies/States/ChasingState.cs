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
    private EnemyController _enemyController;

    private float _loseProgress;

    private float _distanceToPlayer;
    private bool _playerInSight;

    public ChasingState(Sensor[] sensors, Transform currentTransform, EnemyController enemyController, NavMeshAgent navMeshAgent, Enemy_ScriptableObject enemyConfig, EnemyAnimator enemyAnimator)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyController = enemyController;
        _navMeshAgent = navMeshAgent;   
        _enemyConfig = enemyConfig;
        _enemyAnimator = enemyAnimator;
    }

    public bool PlayerLost() => _loseProgress >= _enemyConfig.enemyLostPlayerTime;
    public bool PlayerInAttackRange() => _playerInSight && (_distanceToPlayer < _enemyConfig.enemyAttackRangeMin);



    public override void Enter()
    {
        base.Enter();
        _loseProgress = 0;
        _playerInSight = false;
        _navMeshAgent.stoppingDistance = _enemyConfig.enemyAttackRangeMin;
        _enemyAnimator.StartMove(_enemyConfig.enemyChaseSpeed);
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
                _enemyController.RotateToPoint((Vector3)sensor.SignalSourcePosition, 5f);
                _chasingPosition = sensor.SignalSourcePosition;
                break;
            }
        }
        if (_enemyController.IsPointReached() && !anySensorIsActive)
        {
            _loseProgress += Time.deltaTime;
        }
        if (_chasingPosition == null)
        {
            return;
        }
        _enemyController.RunToPoint(_chasingPosition.Value, _enemyConfig.enemyChaseSpeed);
    }
}