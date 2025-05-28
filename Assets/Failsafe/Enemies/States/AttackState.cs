using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Атака лучем объекта, попавшего в сенсор
/// Противник кастует луч в сторону игрока, опираясь на визуальный сенсор
/// </summary>
public class AttackState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;
    private Vector3? _targetPosition;

    //Параметры луча атаки
    private float _attackDelay = 3f;
    private float _rayDuration = 5f;
    private float _rayDPS = 5f;
    private float _rayCooldown = 10f;
    private float _attackProgress = 0;
    private bool _delayOver = false;
    private bool _onCooldown = false;

    private EnemyController _enemyController;

    private float _attackRangeMax = 15f;
    private float _distanceToPlayer;

    public AttackState(Sensor[] sensors, Transform currentTransform, EnemyController enemyController)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyController = enemyController;
    }

    public bool PlayerOutOfAttackRange() => _distanceToPlayer > _attackRangeMax;

    public override void Enter()
    {
        base.Enter();
        _attackProgress = 0;
        _delayOver = false;
        _onCooldown = false;
        Debug.Log("Enter AttackState");
    }

    public override void Update()
    {
        if (!_delayOver && _attackProgress > _attackDelay)
        {
            _delayOver = true;
            _attackProgress = 0;
        }

        foreach (var sensor in _sensors)
        {
            if (sensor.GetType() == typeof(VisualSensor) && sensor.IsActivated())
            {
                _distanceToPlayer = ((Vector3)(sensor.SignalSourcePosition - _transform.position)).magnitude;

                _targetPosition = sensor.SignalSourcePosition;
                _enemyController.RotateToPoint(_targetPosition.Value, _transform.up);
                if (_delayOver && !_onCooldown)
                {
                    Debug.Log("Пиу");
                    if(sensor.SignalInAttackRay((Vector3)_targetPosition))
                        Debug.Log("Попал"); //Сюда прикрутить здоровье
                }
            }
        }

        if (_attackProgress > _rayDuration)
        {
            _onCooldown = true;
            Debug.Log("Пиу КД");
        }

        if (_attackProgress > _rayDuration + _rayCooldown)
        {
            _onCooldown = false;
            _attackProgress = 0;
        }

        _attackProgress += Time.deltaTime;
    }
}