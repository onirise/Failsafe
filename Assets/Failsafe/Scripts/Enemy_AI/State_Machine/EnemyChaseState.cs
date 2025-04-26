using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private NavMeshAgent _navMeshAgent;
    private FieldOfView _fieldOfView;
    private ZonesOfHearing _zonesOfHearing;
    private bool isChasing = false;
    float _lostPlayerTimer; // Таймер потери игрока
    /// <summary>
    /// Выполняется при входе в состояние преследования.
    /// </summary>
    public override void EnterState(EnemyStateMachine enemy)
    {
        InizialezeComponents(enemy);
        _navMeshAgent.speed = enemy.enemyChaseSpeed;
        isChasing = true;
        Debug.Log("Entering Chase State");
    }

    /// <summary>
    /// Выполняется при выходе из состояния преследования.
    /// </summary>
    public override void ExitState(EnemyStateMachine enemy)
    {
        Debug.Log("Exiting Chase State");
    }

    /// <summary>
    /// Обновляет логику состояния преследования.
    /// </summary>
    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (_fieldOfView == null || _zonesOfHearing == null) return;

        if (_fieldOfView.canSeePlayer)
        {
            _lostPlayerTimer = 5f; // Сброс таймера
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer(enemy);
        }
        else
        {
            LosePlayer(enemy);
        }
        AttackStateSwitch(enemy);
    }

    /// <summary>
    /// Логика преследования игрока.
    /// </summary>
    private void ChasePlayer(EnemyStateMachine enemy)
    {
        if (_navMeshAgent == null || enemy.player == null) return;

        if (_navMeshAgent.destination != enemy.player.transform.position)
        {
            _navMeshAgent.SetDestination(enemy.player.transform.position);
        }
    }

    /// <summary>
    /// Логика потери игрока.
    /// </summary>
    private void LosePlayer(EnemyStateMachine enemy)
    {
        if (_navMeshAgent == null) return;

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
        isChasing = false;
        _lostPlayerTimer = enemy.lostPlayerTimer; // Сброс таймера
        enemy.afterChase = true; // Установить флаг после преследования
        enemy.EnemySwitchState(enemy.searchState); // Переход в состояние поиска
    }

    private void AttackStateSwitch(EnemyStateMachine enemy)
    {
        if (_zonesOfHearing.playerNear)
        {
            enemy.EnemySwitchState(enemy.attackState);
        }
    }

    private void InizialezeComponents(EnemyStateMachine enemy)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        _fieldOfView = enemy.GetComponent<FieldOfView>();
        _zonesOfHearing = enemy.GetComponent<ZonesOfHearing>();
        Debug.Log("Все необходимые компоненты найдены.");

        if (_navMeshAgent == null || _fieldOfView == null || _zonesOfHearing == null)
        {
            Debug.LogError("Не удалось найти необходимые компоненты на объекте врага.");
            return;
        }
       
        
    }
}
