using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{ 
    float _lostPlayerTimer; // Таймер потери игрока

    /// <summary>
    /// Выполняется при входе в состояние преследования.
    /// </summary>
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.Agent.speed = 8f; // Установка скорости преследования
        enemy.Agent.isStopped = false; // Разрешить движение
        _lostPlayerTimer = enemy.lostPlayerTimer; // Инициализация таймера потери игрока


    }

    /// <summary>
    /// Выполняется при выходе из состояния преследования.
    /// </summary>
    public override void ExitState(EnemyStateMachine enemy)
    {

    }

    /// <summary>
    /// Обновляет логику состояния преследования.
    /// </summary>
    public override void UpdateState(EnemyStateMachine enemy)
    {        
         if(enemy.FOV.canSeePlayerFar || enemy.FOV.canSeePlayerNear)
        {
            ChasePlayer(enemy); // Логика преследования игрока
           
        } 
        else
        {
            LosePlayer(enemy); // Логика потери игрока
        }


    }

    /// <summary>
    /// Логика преследования игрока.
    /// </summary>
    private void ChasePlayer(EnemyStateMachine enemy)
    {
        if (enemy.Agent == null || enemy.player == null) return;

        if (enemy.Agent.destination != enemy.player.transform.position)
        {
            enemy.Agent.SetDestination(enemy.player.transform.position);
        }
    }

    /// <summary>
    /// Логика потери игрока.
    /// </summary>
    private void LosePlayer(EnemyStateMachine enemy)
    {
        if (enemy.Agent == null) return;

        if (_lostPlayerTimer > 0)
        {
            _lostPlayerTimer -= Time.deltaTime;
            ChasePlayer(enemy); // Продолжать движение к игроку
        }
        else
        {
            ResetChaseState(enemy);
        }
    }

    /// <summary>
    /// Сброс состояния преследования.
    /// </summary>
    private void ResetChaseState(EnemyStateMachine enemy)
    {
        _lostPlayerTimer = enemy.lostPlayerTimer; // Сброс таймера
        enemy.afterChase = true; // Установить флаг после преследования
        enemy.searchingPoint = enemy.transform.position; // Установить точку поиска
        enemy.SwitchState<EnemyPatrolingState>(); // Переключиться на состояние поиска
    }

    private void AttackStateSwitch(EnemyStateMachine enemy)
    {
        UnityEngine.Debug.Log("Switching to Attack State");
    }
}
