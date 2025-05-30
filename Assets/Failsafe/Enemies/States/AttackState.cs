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
    private EnemyAnimator _enemyAnimator;


    private float _attackRangeMax = 15f;
    private float _distanceToPlayer;

    private LaserBeamController _activeLaser;
    private GameObject _laserPrefab;
    private Transform _laserOrigin;

    public AttackState(Sensor[] sensors, Transform currentTransform, EnemyController enemyController, EnemyAnimator enemyAnimator, LaserBeamController laserBeamController, GameObject laser, Transform laserOrigin)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyController = enemyController;
        _enemyAnimator = enemyAnimator;
        _activeLaser = laserBeamController;
        _laserPrefab = laser;
        _laserOrigin = laserOrigin;
    }

    public bool PlayerOutOfAttackRange() => _distanceToPlayer > _attackRangeMax;

    public override void Enter()
    {
        base.Enter();
        _attackProgress = 0;
        _delayOver = false;
        _onCooldown = false;
        Debug.Log("Enter AttackState");
        _enemyAnimator.UseRootRotation = false; // Отключаем ротацию по корню, чтобы атака была более естественной

        if (_laserPrefab == null)
        {
            Debug.LogError("Laser Prefab не назначен!");
            return;
        }
        if (_laserOrigin == null)
        {
            Debug.LogError("Laser Origin не назначен!");
            return;
        }
    }

    public override void Update()
    {
        _attackProgress += Time.deltaTime;

        if (!_delayOver && _attackProgress > _attackDelay)
        {
            _delayOver = true;
            _attackProgress = 0;
        }

        foreach (var sensor in _sensors)
        {
            if (sensor is VisualSensor && sensor.IsActivated())
            {
                _distanceToPlayer = Vector3.Distance(sensor.SignalSourcePosition.Value, _transform.position);
                _targetPosition = sensor.SignalSourcePosition;
                _enemyController.RotateToPoint(_targetPosition.Value);


                if (_delayOver && !_onCooldown && !_enemyAnimator.IsInAction())
                {
                    if (_activeLaser == null)
                    {
                        GameObject laserGO = GameObject.Instantiate(_laserPrefab, _laserOrigin.position, _laserOrigin.rotation);
                        _activeLaser = laserGO.GetComponent<LaserBeamController>();
                        _activeLaser.Initialize(_laserOrigin, _targetPosition.Value);
                    }

                    _enemyAnimator.TryAttack(); // ← Запускаем анимацию атаки

                    if (sensor.SignalInAttackRay(_targetPosition.Value))
                    {
                        Debug.Log("Попал"); // сюда добавить урон
                       
                    }
                }
            }
        }

        if (_attackProgress > _rayDuration)
        {
            if (_activeLaser != null)
            {
                GameObject.Destroy(_activeLaser.gameObject);
                _activeLaser = null;
            }
            _onCooldown = true;
            _enemyAnimator.TryReload(); // ← Анимация перезарядки
            _enemyAnimator.isReloading(true); // Устанавливаем флаг перезарядки    
            Debug.Log("Атака на перезарядке");
        }

        if (_attackProgress > _rayDuration + _rayCooldown)
        {
            _onCooldown = false;
            _enemyAnimator.isReloading(false); // Сбрасываем флаг перезарядки
            _attackProgress = 0;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enemyAnimator.UseRootRotation = true; // Включаем ротацию по root
    }
}