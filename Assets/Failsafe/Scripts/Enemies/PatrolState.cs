using FMOD;
using System;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class PatrolState : BehaviorState
{
    private Transform _transform;
    private Sensor[] _sensors;
    private Vector3 _patrolPoint;
    private Transform[] _patrolPoints;
    private EnemyController _enemyController;
    private float _offset = 10f;
    private float _waitTime = 3f;
    private float _waitTimer;
    private int _currentPatrolPointIndex = 0;
    private bool _isWaiting = false;
    public PatrolState(Sensor[] sensors, Transform transform, EnemyController enemyController, Transform[] patrolPoints = null)
    {
        _transform = transform;
        _sensors = sensors;
        _patrolPoints = patrolPoints;
        _enemyController = enemyController;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter PatrolState");

        _waitTimer = _waitTime; // Инициализировать таймер
        _isWaiting = false;

        HandlePatrolling(); // Установит _patrolPoint и начнет движение
    }

    private float _warningProgress;
    private float _warningTime = 1;

    public bool PlayerSpotted() => _warningProgress >= _warningTime;

    public override void Update()
    {

        foreach (var sensor in _sensors)
        {
            if (sensor.IsActivated())
            {
                _enemyController.RotateToPoint(sensor.SignalSourcePosition.Value, Vector3.up);
                _warningProgress += sensor.SignalStrength * Time.deltaTime;
            }
        }

        if (_isWaiting)
        {
            HandleWaiting();
        }
        else
        {
            if (_enemyController.IsPointReached())
            {
                _isWaiting = true;
                _enemyController.StopMoving();
            }
            else
            {
                HandlePatrolling();
            }
        }
    }

    public bool isThereAnyPatrolPoint()
    {
        return _patrolPoints != null && _patrolPoints.Length > 0;
    }

    private void HandlePatrolling()
    {
        if (isThereAnyPatrolPoint())
        {
            // Обновляем индекс
            _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % _patrolPoints.Length;

            // Сохраняем следующую точку
            _patrolPoint = _patrolPoints[_currentPatrolPointIndex].position;

            // Отдаем команду двигаться
            _enemyController.MoveToPoint(_patrolPoint);
        }
        else
        {
            _patrolPoint = _enemyController.RandomPoint();
        }
    }

    private void HandleWaiting()
    {
        _waitTimer -= Time.deltaTime;

        if (_waitTimer <= 0f)
        {
            _waitTimer = _waitTime;
            _isWaiting = false;
            _enemyController.MoveToPoint(_patrolPoint);
        }
    }
}
