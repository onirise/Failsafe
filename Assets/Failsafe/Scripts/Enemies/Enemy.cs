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

        var defaultState = new DefaultState(_sensors, transform);
        var chasingState = new ChasingState(_sensors, transform, navMeshAgent);

        defaultState.AddTransition(chasingState, defaultState.PlayerSpoted);
        chasingState.AddTransition(defaultState, chasingState.PlayerLost);

        _stateMachine = new BehaviorStateMachine(defaultState);
    }

    void Update()
    {
        _stateMachine.Update();
    }
}
