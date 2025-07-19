using UnityEngine;

public class CheckState : BehaviorState
{
    private Vector3 _originPoint;
    private Vector3 _targetPoint;
    private Vector3 _searchDirection;
    private float _checkTimer;

    private bool _hasReachedOrigin;
    private bool _isWaiting;
    private float _waitTimer;

    private Sensor[] _sensors;
    private EnemyController _enemyController;
    private Enemy_ScriptableObject _config;
    private Transform _transform;
    
    public bool CheckEnd() => _checkTimer >= _config.CheckDuration;
    public CheckState(Sensor[] sensors, Transform transform, EnemyController enemyController, Enemy_ScriptableObject config)
    {
        _sensors = sensors;
        _transform = transform;
        _enemyController = enemyController;
        _config = config;
    }

    public override void Enter()
    {
        base.Enter();
        _hasReachedOrigin = false;
        _isWaiting = false;
        _waitTimer = 0f;
        _checkTimer = 0f;

        // Берём первую активную точку сигнала
        foreach (var sensor in _sensors)
        {
            if (sensor.IsActivated() && sensor.SignalSourcePosition.HasValue)
            {
                _originPoint = sensor.SignalSourcePosition.Value;
                _searchDirection = (sensor.SignalSourcePosition.Value - _transform.position).normalized;
                _enemyController.MoveToPoint(_originPoint, _config.PatrolingSpeed);
                break;
            }
        }
    }

    public override void Update()
    {
        base.Update();

        if (!_hasReachedOrigin)
        {
            if (_enemyController.IsPointReached())
            {
                _hasReachedOrigin = true;
                _isWaiting = true;
                _waitTimer = _config.PatrollingWaitTime;
            }
            return;
        }

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                PickPoint(_transform.position);
            }
            return;
        }

        if (_enemyController.IsPointReached())
        {
            _checkTimer += Time.deltaTime;
            _isWaiting = true;
            _waitTimer = _config.changePointInterval;
        }
    }

    private void PickPoint(Vector3 center)
    {
        _targetPoint = _enemyController.RandomPointAround(_originPoint, _config.CheckRadius);
        _enemyController.MoveToPoint(_targetPoint, _config.PatrolingSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        _isWaiting = false;
        _waitTimer = 0f;
        _checkTimer = 0f;
    }
}