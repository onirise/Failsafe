using DMDungeonGenerator;
using SteamAudio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Sensor[] _sensors;
    private BehaviorStateMachine _stateMachine;
    private AwarenessMeter _awarenessMeter;
    private Animator _animator;
    private EnemyAnimator _enemyAnimator;
    private EnemyController _controller;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        // Основные компоненты
        _animator = GetComponent<Animator>();
        _sensors = GetComponents<Sensor>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Отключаем автоматическое управление трансформацией
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;

        // Создаём вспомогательные классы
        _controller = new EnemyController(transform, _navMeshAgent);
        _awarenessMeter = new AwarenessMeter(_sensors);
        _enemyAnimator = new EnemyAnimator(_navMeshAgent, _animator, transform, _controller);
    }

    private void Start()
    {
        

        // Создаём состояния (уже можно брать патрульные точки из Room)
        var defaultState = new DefaultState(_sensors, transform, _controller);
        var chasingState = new ChasingState(_sensors, transform, _controller);
        var patrolState = new PatrolState(_sensors, _controller);

        defaultState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        patrolState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        defaultState.AddTransition(patrolState, defaultState.IsPatroling);
        chasingState.AddTransition(patrolState, chasingState.PlayerLost);

        var disabledStates = new List<BehaviorForcedState> { new DisabledState() };
        _stateMachine = new BehaviorStateMachine(defaultState, disabledStates);

        // Находим комнату и настраиваем поведение
        RoomCheck();

    }

    void Update()
    {
        _enemyAnimator.UpdateAnimator();
        _stateMachine.Update();
        _awarenessMeter.Update();
    }

    [ContextMenu("DisableState")]
    public void DisableState()
    {
        _stateMachine.ForseChangeState<DisabledState>();
    }

    private void RoomCheck()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f);

        foreach (var hit in hits)
        {
            var room = hit.GetComponent<RoomData>() ?? hit.GetComponentInParent<RoomData>();
            if (room != null)
            {
                _controller.SetCurrentRoom(room);
                break;
            }
        }

        var points = _controller.GetRoomPatrolPoints();
        Debug.Log($"[Enemy] Получено точек патруля: {points.Count}");
    }

    void OnAnimatorMove()
    {
        _enemyAnimator.ApplyRootMotion(); // Всё управление Root Motion'ом теперь централизовано здесь
        // Только rotation — из Root Motion
        transform.rotation = _animator.rootRotation;

        // Берём позицию от NavMeshAgent (в том числе Y)
        transform.position = _navMeshAgent.nextPosition;
    }
}

