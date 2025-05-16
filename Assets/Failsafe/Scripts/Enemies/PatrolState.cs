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
    private float offset = 10f;
    private float _waitTime = 5f;
    private float _waitTimer;
    private int currentPatrolPointIndex = 0;
    private bool isWaiting = false;
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
        isWaiting = false;

        HandlePatrolling(); // Установит _patrolPoint и начнет движение
    }

    public override void Update()
    {
        if (isWaiting)
        {
            HandleWaiting();
        }
        else
        {
            if (_enemyController.IsPointReached())
            {
                isWaiting = true;
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
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % _patrolPoints.Length;

            // Сохраняем следующую точку
            _patrolPoint = _patrolPoints[currentPatrolPointIndex].position;

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
            isWaiting = false;
            _enemyController.MoveToPoint(_patrolPoint);
        }
    }
}
