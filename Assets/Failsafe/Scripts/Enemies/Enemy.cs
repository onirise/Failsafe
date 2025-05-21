using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Sensor[] _sensors;
    private BehaviorStateMachine _stateMachine;
    
    void Start()
    {
        _sensors = GetComponents<Sensor>();
        var navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        var enemyController = new EnemyController(this, this.transform, navMeshAgent);
        var awareness = GetComponent<AwarenessMeter>();

        var defaultState = new DefaultState(_sensors, transform, enemyController);
        var chasingState = new ChasingState(_sensors, transform, enemyController);
        var patrolState = new PatrolState(_sensors, transform, enemyController);

        defaultState.AddTransition(chasingState, awareness.IsChasing);
        chasingState.AddTransition(patrolState, awareness.IsAlerted);
         patrolState.AddTransition(chasingState, awareness.IsChasing);

        var disabledStates = new List<BehaviorForcedState>() { new DisabledState() };

        _stateMachine = new BehaviorStateMachine(defaultState, disabledStates);
    }

    void Update()
    {
        _stateMachine.Update();
    }

    [ContextMenu("DisableState")]
    public void DisableState()
    {
        _stateMachine.ForseChangeState<DisabledState>();
    }
}
