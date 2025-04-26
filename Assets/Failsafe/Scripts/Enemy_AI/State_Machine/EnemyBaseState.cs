using UnityEngine;

public abstract class EnemyBaseState
{
    abstract public void EnterState(EnemyStateMachine enemy);
    abstract public void UpdateState(EnemyStateMachine enemy);
    abstract public void ExitState(EnemyStateMachine enemy);
}
