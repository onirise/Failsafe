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
    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    private Enemy_ScriptableObject _enemyConfig;
    
    private float _attackProgress = 0;
    private bool _delayOver = false;
    private bool _onCooldown = false;
    private bool _attackFired = false;

    private EnemyController _enemyController;
    private EnemyAnimator _enemyAnimator;

    private float _distanceToPlayer;

    private LaserBeamController _activeLaser;
    private GameObject _laserPrefab;
    private Transform _laserOrigin;
    private bool _playerInSight;

    public AttackState(Sensor[] sensors, Transform currentTransform, EnemyController enemyController, EnemyAnimator enemyAnimator, LaserBeamController laserBeamController, GameObject laser, Transform laserOrigin,NavMeshAgent navMeshAgent ,Enemy_ScriptableObject enemyconfig)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyController = enemyController;
        _enemyAnimator = enemyAnimator;
        _activeLaser = laserBeamController;
        _laserPrefab = laser;
        _laserOrigin = laserOrigin;
        _navMeshAgent = navMeshAgent;
        _enemyConfig = enemyconfig;
    }

    public bool PlayerOutOfAttackRange()
    {
        return (_distanceToPlayer > _enemyConfig.AttackRangeMax || !_playerInSight) && !_onCooldown;
    }

    public override void Enter()
    {
        base.Enter();
        _attackProgress = 0;
        _delayOver = false;
        _onCooldown = false;
        _attackFired = false;
        _playerInSight = true;
        _enemyController.StopMoving();
        _enemyAnimator.isAttacking(true);
        Debug.Log("Enter AttackState");
    }

    public override void Update()
    {
        _attackProgress += Time.deltaTime;

        if (!_delayOver && _attackProgress > _enemyConfig.AttackDelay)
        {
            _delayOver = true;
            _attackProgress = 0;
        }

        foreach (var sensor in _sensors)
        {
            if (sensor is VisualSensor visual)
                if(visual.IsActivated())
                {
                    _playerInSight = true;
                    _targetPosition = visual.SignalSourcePosition;
                    _distanceToPlayer = Vector3.Distance(_transform.position, _targetPosition.Value);
                    _enemyController.RotateToPoint(_targetPosition.Value, 5f);

                    if (_delayOver && !_onCooldown)
                    {
                        if (_activeLaser == null)
                        {
                            GameObject laserGO = GameObject.Instantiate(_laserPrefab, _laserOrigin.position, _laserOrigin.rotation);
                            _activeLaser = laserGO.GetComponent<LaserBeamController>();
                            _activeLaser.Initialize(_laserOrigin, _targetPosition.Value);
                        }

                        _enemyAnimator.TryAttack();
                        _attackFired = true;

                        var damageableComponent = visual.Target.GetComponentInChildren<DamageableComponent>();
                        if (sensor.SignalInAttackRay((Vector3)_targetPosition) && damageableComponent is not null)
                        {
                            damageableComponent.TakeDamage(new FlatDamage(_enemyConfig.Damage * Time.deltaTime));
                            Debug.Log($"Урон: {_enemyConfig.Damage * Time.deltaTime:F1}" );
                        }
                    }
                }
                else
                {
                    _playerInSight = false;
                }
        }

        if (_attackFired && _attackProgress > _enemyConfig.AttackDuration)
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

        if (_attackProgress > _enemyConfig.AttackDuration + _enemyConfig.AttackCooldown)
        {
            _onCooldown = false;
            _enemyAnimator.isReloading(false);
            _attackProgress = 0;
            _attackFired = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (_activeLaser != null)
        {
            GameObject.Destroy(_activeLaser.gameObject);
            _activeLaser = null;
        }
        _enemyAnimator.isAttacking(false);
        _enemyController.ResumeMoving();
    }
}
