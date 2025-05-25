using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Sensor[] _sensors;
    private BehaviorStateMachine _stateMachine;
    private AwarenessMeter _awarenessMeter;
    void Start()
    {
        _sensors = GetComponents<Sensor>();
        var navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        var enemyController = new EnemyController(this, this.transform, navMeshAgent);
        _awarenessMeter = new AwarenessMeter(_sensors);

        var defaultState = new DefaultState(_sensors, transform, enemyController);
        var chasingState = new ChasingState(_sensors, transform, enemyController);
        var patrolState = new PatrolState(_sensors, transform, enemyController);

        defaultState.AddTransition(chasingState, _awarenessMeter.IsChasing);
        chasingState.AddTransition(patrolState, _awarenessMeter.IsAlerted);
         patrolState.AddTransition(chasingState, _awarenessMeter.IsChasing);

        var disabledStates = new List<BehaviorForcedState>() { new DisabledState() };

        _stateMachine = new BehaviorStateMachine(defaultState, disabledStates);
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
}
