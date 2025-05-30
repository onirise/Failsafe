using DMDungeonGenerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SteamAudio;

public class Enemy : MonoBehaviour
{
    private Sensor[] _sensors;
    private BehaviorStateMachine _stateMachine;
    private AwarenessMeter _awarenessMeter;
    private Animator _animator;
    private EnemyAnimator _enemyAnimator;
    private EnemyController _controller;
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private GameObject _laserBeamPrefab;
    private LaserBeamController _activeLaser;
    [SerializeField] private Transform _laserSpawnPoint; // Точка спавна лазера, если нужно

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
        var attackState = new AttackState(_sensors, transform, _controller, _enemyAnimator, _activeLaser, _laserBeamPrefab, _laserSpawnPoint);

        defaultState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        patrolState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        defaultState.AddTransition(patrolState, defaultState.IsPatroling);
        chasingState.AddTransition(patrolState, chasingState.PlayerLost);
        chasingState.AddTransition(attackState, chasingState.PlayerInAttackRange);
        attackState.AddTransition(chasingState, attackState.PlayerOutOfAttackRange);

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
        // Ищем все коллайдеры рядом с врагом
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f); // можно увеличить радиус при необходимости
        Debug.Log($"[Enemy] Обнаружено коллайдеров: {hits.Length}");

        foreach (var hit in hits)
        {
            Debug.Log($"[Enemy] Hit: {hit.name}");

            // Ищем RoomData в дочерних объектах каждого найденного объекта
            RoomData room = hit.GetComponentInChildren<RoomData>();
            if (room != null)
            {
                Debug.Log($"[Enemy] НАШЁЛ КОМНАТУ через OverlapSphere: {room.name}");
                _controller.SetCurrentRoom(room);
                break;
            }
        }

        // Получаем патрульные точки из установленной комнаты
        var points = _controller.GetRoomPatrolPoints();
        Debug.Log($"[Enemy] Получено точек патруля: {points.Count}");
    }

    void OnAnimatorMove()
    {
        _enemyAnimator.ApplyRootMotion(); // Всё управление Root Motion'ом теперь централизовано здесь
       
    }
}


