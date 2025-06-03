using System.Linq;
using Failsafe.Scripts.Damage;
using Failsafe.Scripts.Damage.Implementation;
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

    private float _attackDelay = 3f;
    private float _rayDuration = 5f;
    private float _rayDPS = 100f;
    private float _rayCooldown = 10f;
    private float _attackProgress = 0;
    private bool _delayOver = false;
    private bool _onCooldown = false;
    private bool _attackFired = false;

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

    public bool PlayerOutOfAttackRange()
    {
        return _distanceToPlayer > _attackRangeMax || _targetPosition == null;
    }

    public override void Enter()
    {
        base.Enter();
        _enemyController.StopMoving();
        _attackProgress = 0;
        _delayOver = false;
        _onCooldown = false;
        _attackFired = false;
        Debug.Log("Enter AttackState");

     
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
            if (sensor is VisualSensor visual && visual.IsActivated())
            {
                _targetPosition = visual.SignalSourcePosition;

                _distanceToPlayer = Vector3.Distance(_transform.position, _targetPosition.Value);
                _enemyController.RotateToPoint(_targetPosition.Value, 5f);
 

                if (_delayOver && !_onCooldown && !_enemyAnimator.IsInAction() && _distanceToPlayer <= _attackRangeMax)
                {
                    Debug.Log("Пиу");

                    var damageableComponent = visual.Target.GetComponentInChildren<DamageableComponent>();

                    if (sensor.SignalInAttackRay((Vector3)_targetPosition) && damageableComponent is not null)
                    {
                        Debug.Log("damage");

                        damageableComponent.TakeDamage(new FlatDamage(_rayDPS * Time.deltaTime));
                    }

                    if (_activeLaser == null)
                    {
                        GameObject laserGO = GameObject.Instantiate(_laserPrefab, _laserOrigin.position, _laserOrigin.rotation);
                        _activeLaser = laserGO.GetComponent<LaserBeamController>();
                        _activeLaser.Initialize(_laserOrigin, _targetPosition.Value);
                    }

                    _enemyAnimator.TryAttack();
                    _attackFired = true;

                    if (sensor.SignalInAttackRay(_targetPosition.Value))
                    {
                        Debug.Log("Попал");

                        _targetPosition = sensor.SignalSourcePosition;

                      
                    }
                }

                if (_attackFired && _attackProgress > _rayDuration)
                {
                    if (_activeLaser != null)
                    {
                        GameObject.Destroy(_activeLaser.gameObject);
                        _activeLaser = null;
                    }

                    _onCooldown = true;
                    _enemyAnimator.TryReload();
                    _enemyAnimator.isReloading(true);
                    Debug.Log("Атака на перезарядке");
                }

                if (_attackProgress > _rayDuration + _rayCooldown)
                {
                    _onCooldown = false;
                    _enemyAnimator.isReloading(false);
                    _attackProgress = 0;
                    _attackFired = false;
                }

            }
        }
    }

}
