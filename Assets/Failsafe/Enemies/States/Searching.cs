using UnityEngine;
using UnityEngine.AI;

public class SearchingState : BehaviorState
{
    private Vector3 _targetPoint;
    private float _searchTimer;
    private Vector3 _searchOrigin;
    private Vector3 _searchDir;
    private bool _hasReachedOrigin = false;

    private float _waitTimer;
    private bool _isWaiting = false;

    private Sensor[] _sensors;
    private Transform _transform;
    private NavMeshAgent _navMeshAgent;
    private Enemy_ScriptableObject _enemyConfig;
    private EnemyMovePatterns _enemyMovePatterns;
    private EnemyNavMeshActions _enemyNavMeshActions;
    private EnemyMemory _enemyMemory;

    public bool SearchingEnd() => _searchTimer >= _enemyConfig.SearchingDuration;

    public SearchingState(Sensor[] sensors, Transform currentTransform, EnemyMovePatterns enemyMovePatterns,EnemyNavMeshActions enemyNavMeshActions,EnemyMemory enemyMemory, NavMeshAgent navMeshAgent, Enemy_ScriptableObject enemyConfig)
    {
        _sensors = sensors;
        _transform = currentTransform;
        _navMeshAgent = navMeshAgent;
        _enemyConfig = enemyConfig;
        _enemyMovePatterns = enemyMovePatterns;
        _enemyNavMeshActions = enemyNavMeshActions;
        _enemyMemory = enemyMemory;
    }

    public override void Enter()
    {
        base.Enter();
        _hasReachedOrigin = false;
        _searchTimer = 0f;
        _isWaiting = false;
        _waitTimer = 0f;

        _navMeshAgent.stoppingDistance = 1f;
        _searchOrigin = _enemyMemory.LastKnownPlayerPosition;
        _searchDir = _enemyMemory.LastKnownPlayerDirection;
        _enemyNavMeshActions.MoveToPoint(_searchOrigin, _enemyConfig.SearchingSpeed);

        Debug.Log("Enter SearchingState: going to last known player position");
    }

    public override void Update()
    {
        base.Update();

        if (!_hasReachedOrigin)
        {
            if (_enemyNavMeshActions.IsPointReached())
            {
                _hasReachedOrigin = true;
                _isWaiting = true;
                _waitTimer = _enemyConfig.PatrollingWaitTime;
                Debug.Log("Reached last known player position, starting search phase");
            }
            return;
        }

        _searchTimer += Time.deltaTime;

        if (SearchingEnd())
            return;

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

        if (_enemyNavMeshActions.IsPointReached())
        {
            _isWaiting = true;
            _waitTimer = _enemyConfig.changePointInterval;
        }
    }

    private void PickPoint(Vector3 center)
    {
        _targetPoint = _enemyMovePatterns.RandomPointInForwardCone(_searchOrigin, _searchDir, _enemyConfig.SearchRadius, 65f);
        _enemyNavMeshActions.MoveToPoint(_targetPoint, _enemyConfig.SearchingSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        _searchTimer = 0f;
        _isWaiting = false;
        _waitTimer = 0f;
    }
}
