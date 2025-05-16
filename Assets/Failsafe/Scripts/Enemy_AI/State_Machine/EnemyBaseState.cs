using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateMachine enemy); // Заходит в состояние
    public abstract void UpdateState(EnemyStateMachine enemy); // Обновляет логику состояния
    public abstract void ExitState(EnemyStateMachine enemy); // Выходит из состояния
}