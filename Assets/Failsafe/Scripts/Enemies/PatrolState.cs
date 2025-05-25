using DMDungeonGenerator;
using FMOD;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using Debug = UnityEngine.Debug;

public class PatrolState : BehaviorState
{
    private readonly Transform _transform;
    private readonly Sensor[] _sensors;
    private readonly EnemyController _enemyController;

    private List<Transform> _patrolPoints = new();
    private int _currentPatrolPointIndex = -1;
    private Vector3 _patrolPoint;

    private float _waitTime = 3f;
    private float _waitTimer;
    private bool _isWaiting = false;
    private float _warningProgress;
    private float _warningTime = 1f;

    public PatrolState(Sensor[] sensors, Transform transform, EnemyController enemyController)
    {
        _sensors = sensors;
        _transform = transform;
        _enemyController = enemyController;
    }

    public override void Enter()
    {
        base.Enter();
        _waitTimer = _waitTime;
        _isWaiting = false;
        _warningProgress = 0f;
        Debug.Log($"[PatrolState] Текущая комната: {_enemyController.CurrentRoom?.name ?? "NULL"}");
        var points = _enemyController.GetRoomPatrolPoints();
        Debug.Log($"[PatrolState] Получено точек: {points.Count}");

        _patrolPoints = points;
        // Берём точки патруля из текущей комнаты
        _patrolPoints = _enemyController.GetRoomPatrolPoints();

        if (_patrolPoints == null || _patrolPoints.Count == 0)
        {
            Debug.LogWarning($"[PatrolState] {_transform.name} нет патрульных точек — fallback.");
            _patrolPoint = _enemyController.RandomPoint();
            _enemyController.MoveToPoint(_patrolPoint);
        }
        else
        {
            _currentPatrolPointIndex = -1;
            HandlePatrolling();
        }
    }

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
        else if (_enemyController.IsPointReached())
        {
            _isWaiting = true;
            _enemyController.StopMoving();
        }
    }

    private void HandleWaiting()
    {
        _waitTimer -= Time.deltaTime;
        if (_waitTimer <= 0f)
        {
            _waitTimer = _waitTime;
            _isWaiting = false;
            HandlePatrolling();
        }
    }

    private void HandlePatrolling()
    {
        if (_patrolPoints == null || _patrolPoints.Count == 0)
        {
            _patrolPoint = _enemyController.RandomPoint();
        }
        else
        {
            _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % _patrolPoints.Count;
            _patrolPoint = _patrolPoints[_currentPatrolPointIndex].position;
        }

        _enemyController.MoveToPoint(_patrolPoint);
    }

    public bool PlayerSpotted() => _warningProgress >= _warningTime;
}