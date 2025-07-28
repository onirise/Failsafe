using Failsafe.Scripts.Damage;
using Failsafe.Scripts.Damage.Implementation;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;
    private Transform _target;
    private Transform _targetPoint;
    private NavMeshAgent _navMeshAgent;
    private Enemy_ScriptableObject _enemyConfig;

    private float _attackProgress = 0;
    private bool _delayOver = false;
    private bool _onCooldown = false;
    private bool _attackFired = false;
    private bool _targetPointLocked = false;

    private EnemyNavMeshActions _enemyNavMeshActions;
    private EnemyAnimator _enemyAnimator;

    private float _distanceToPlayer;
    private LaserBeamController _activeLaser;
    private GameObject _laserPrefab;
    private Transform _laserOrigin;
    public AttackState(Sensor[] sensors, Transform currentTransform, EnemyNavMeshActions enemyNavMeshActions, EnemyAnimator enemyAnimator, LaserBeamController laserBeamController, GameObject laser, Transform laserOrigin,NavMeshAgent navMeshAgent ,Enemy_ScriptableObject enemyConfig)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _enemyNavMeshActions = enemyNavMeshActions;
        _enemyAnimator = enemyAnimator;
        _activeLaser = laserBeamController;
        _laserPrefab = laser;
        _laserOrigin = laserOrigin;
        _navMeshAgent = navMeshAgent;
        _enemyConfig = enemyConfig;
    }

    public bool PlayerOutOfAttackRange()
    {
        return (_targetPoint == null || _distanceToPlayer > _enemyConfig.AttackRangeMax)
               && !_onCooldown && !_attackFired;
    }

    public override void Enter()
    {
        base.Enter();
        _attackProgress = 0;
        _delayOver = false;
        _onCooldown = false;
        _attackFired = false;
        _targetPointLocked = false;
        _enemyNavMeshActions.StopMoving();
        _enemyAnimator.isAttacking(true);
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
            if (sensor is VisualSensor visual && visual.IsActivated())
            {
                _target = visual.Target;

                // Зафиксировать точку только один раз
                if (!_targetPointLocked)
                {
    
                    _targetPoint = visual.GetBestVisiblePointWithChestOverride();
                    _targetPointLocked = _targetPoint != null;

                    if (_targetPointLocked)
                        Debug.Log($"🎯 Цель зафиксирована: {_targetPoint.name}");
                }

                if (_targetPoint == null) return;

                _distanceToPlayer = Vector3.Distance(_transform.position, _targetPoint.position);
                _enemyNavMeshActions.RotateToPoint(_targetPoint.position, 5f);

                if (_delayOver && !_onCooldown && !_attackFired)
                {
                    if (_activeLaser == null)
                    {
                        GameObject laserGO = GameObject.Instantiate(_laserPrefab, _laserOrigin.position, _laserOrigin.rotation);
                        _activeLaser = laserGO.GetComponent<LaserBeamController>();
                        _activeLaser.Initialize(_laserOrigin, _targetPoint);
                    }

                    _enemyAnimator.TryAttack();
                    _attackFired = true;

                    var damageable = _target.GetComponentInChildren<DamageableComponent>();
                    if (sensor.SignalInAttackRay(_targetPoint.position) && damageable != null)
                    {
                        damageable.TakeDamage(new FlatDamage(_enemyConfig.Damage * Time.deltaTime));
                    }
                }
            }
        }

        // Завершение атаки
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
        }

        // Завершение кулдауна
        if (_attackProgress > _enemyConfig.AttackDuration + _enemyConfig.AttackCooldown)
        {
            _onCooldown = false;
            _enemyAnimator.isReloading(false);
            _attackProgress = 0;
            _attackFired = false;

            // готов к новой атаке — сбрасываем фиксированную точку
            _targetPoint = null;
            _targetPointLocked = false;
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
        _enemyNavMeshActions.ResumeMoving();
        _targetPoint = null;
        _targetPointLocked = false;
    }
}