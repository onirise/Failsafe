using UnityEngine;

public class EnemyOnDetectPlayerState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.afterChase = true; // Устанавливаем флаг, что враг был в состоянии погони
       
            enemy.SwitchState<EnemyChaseState>(); // Переключаемся на состояние погони

        
    }    
    public override void UpdateState(EnemyStateMachine enemy)
    {
     
    }
    public override void ExitState(EnemyStateMachine enemy)
    {
        // Здесь можно добавить логику, которая выполняется при выходе из состояния
    }
}

    

