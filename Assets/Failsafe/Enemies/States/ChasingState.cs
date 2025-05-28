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

    private EnemyController _enemyController;

    private float _loseTime = 3;
    private float _loseProgress;

    private float _attackRangeMin = 10f;
    private float _distanceToPlayer;

    public ChasingState(Sensor[] sensors, Transform currentTransform, EnemyController enemyController)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyController = enemyController;
    }

    public bool PlayerLost() => _loseProgress >= _loseTime;
    public bool PlayerInAttackRange() => _distanceToPlayer < _attackRangeMin;



    public override void Enter()
    {
        base.Enter();
        _loseProgress = 0;
        Debug.Log("Enter ChasingState");
    }

    public override void Update()
    {
        bool anySensorIsActive = false;
        foreach (var sensor in _sensors)
        {
            if (sensor.GetType() == typeof(VisualSensor) && sensor.IsActivated())
                _distanceToPlayer = ((Vector3)(sensor.SignalSourcePosition - _transform.position)).magnitude;

            if (sensor.IsActivated())
            {
                anySensorIsActive = true;
                _loseProgress = 0;
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
        _enemyController.RunToPoint(_chasingPosition.Value);
        _enemyController.RotateToPoint(_chasingPosition.Value, _transform.up);
    }
}