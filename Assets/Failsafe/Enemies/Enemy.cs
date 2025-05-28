using DMDungeonGenerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Sensor[] _sensors;
    private BehaviorStateMachine _stateMachine;
    private AwarenessMeter _awarenessMeter;
    private EnemyController _controller;
    void Start()
    {
        _sensors = GetComponents<Sensor>();
        var navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        _controller = new EnemyController(this, this.transform, navMeshAgent);
        _awarenessMeter = new AwarenessMeter(_sensors);

        var defaultState = new DefaultState(_sensors, transform, _controller);
        var chasingState = new ChasingState(_sensors, transform, _controller);
        var patrolState = new PatrolState(_sensors, transform, _controller);
        var attackState = new AttackState(_sensors, transform, _controller);

        defaultState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        patrolState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        defaultState.AddTransition(patrolState, defaultState.IsPatroling);
        chasingState.AddTransition(patrolState, chasingState.PlayerLost);
        chasingState.AddTransition(attackState, chasingState.PlayerInAttackRange);
        attackState.AddTransition(chasingState, attackState.PlayerOutOfAttackRange);

        var disabledStates = new List<BehaviorForcedState>() { new DisabledState() };

        _stateMachine = new BehaviorStateMachine(defaultState, disabledStates);
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f); // увеличим радиус для надёжности
        Debug.Log($"[Enemy] Обнаружено коллайдеров: {hits.Length}");

        foreach (var hit in hits)
        {
            Debug.Log($"[Enemy] Hit: {hit.name}");

            if (hit.TryGetComponent<RoomData>(out var room))
            {
                Debug.Log($"[Enemy] НАШЁЛ КОМНАТУ через OverlapSphere: {room.name}");
                _controller.SetCurrentRoom(room);
                break;
            }
        }

        var points = _controller.GetRoomPatrolPoints();
        Debug.Log($"[Enemy] Получено точек патруля: {points.Count}");
    }

    void Update()
    {
        _stateMachine.Update();
        _awarenessMeter.Update();

    }

    [ContextMenu("DisableState")]
    public void DisableState()
    {
        _stateMachine.ForseChangeState<DisabledState>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_controller == null) return;

        if (other.TryGetComponent<RoomData>(out var room))
        {
            _controller.SetCurrentRoom(room);
            Debug.Log($"[Trigger] {name} вошёл в комнату {room.name}");
        }
    }
}



