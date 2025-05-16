using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySearchState : EnemyBaseState
{
    private Vector3 searchPoint;
    private NavMeshAgent _navMeshAgent;
    private FieldOfView _fieldOfView;
    float _timeToGet;
    float _searchDuration;
    float _changePointTimer;

    /// <summary>
    /// Выполняется при входе в состояние поиска.
    /// </summary>
    public override void EnterState(EnemyStateMachine enemy)
    {
        InitializeComponents(enemy);
        ResetAllTimers(enemy);
        enemy.afterChase = false;
        MoveToSearchPoint(enemy.searchingPoint);
    }

    /// <summary>
    /// Выполняется при выходе из состояния поиска.
    /// </summary>
    public override void ExitState(EnemyStateMachine enemy)
    {
        searchPoint = Vector3.zero;
        _navMeshAgent.SetDestination(enemy.transform.position);
        ResetAllTimers(enemy);
    }

    /// <summary>
    /// Обновляет логику состояния поиска.
    /// </summary>
    public override void UpdateState(EnemyStateMachine enemy)
    {
        CantGetToSearchPoint(enemy);
        OnThePoint(enemy);
    }

    /// <summary>
    /// Возвращает врага в состояние патрулирования.
    /// </summary>
    private void BackToPatrol(EnemyStateMachine enemy)
    {
        Debug.Log("Going back to Patrol State");
        enemy.SwitchState<EnemyPatrolingState>();
    }

    /// <summary>
    /// Логика поиска игрока.
    /// </summary>
    private void PerformSearch(EnemyStateMachine enemy)
    {
        Debug.Log("Searching for Player");

        _searchDuration -= Time.deltaTime;
        _changePointTimer -= Time.deltaTime;
        if (_searchDuration <= 0)
        {
            BackToPatrol(enemy);
        }
        else if (_changePointTimer <= 0)
        {
            _changePointTimer = 0.5f;
            Vector3 randomSearchPoint = new Vector3(
                UnityEngine.Random.Range(searchPoint.x - enemy.searchRadius, searchPoint.x + enemy.searchRadius),
                0,
                UnityEngine.Random.Range(searchPoint.z - enemy.searchRadius, searchPoint.z + enemy.searchRadius)
            );

            if (_navMeshAgent.destination != randomSearchPoint)
            {
                _navMeshAgent.SetDestination(randomSearchPoint);
            }
        }
    }

    /// <summary>
    /// Перемещает врага к точке поиска.
    /// </summary>
    private void MoveToSearchPoint(Vector3 pos)
    {
        Debug.Log("Going to Search Point");
        _navMeshAgent.SetDestination(pos);
        _navMeshAgent.speed = 8f;
    }

    /// <summary>
    /// Инициализирует необходимые компоненты.
    /// </summary>
    private void InitializeComponents(EnemyStateMachine enemy)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        _fieldOfView = enemy.GetComponent<FieldOfView>();

        if (_navMeshAgent == null || _fieldOfView == null)
        {
            Debug.LogError("NavMeshAgent или FieldOfView не найдены на объекте врага!");
        }
    }

    /// <summary>
    /// Сбрасывает все таймеры.
    /// </summary>
    private void ResetAllTimers(EnemyStateMachine enemy)
    {
        _timeToGet = enemy.timeToGet;
        _searchDuration = enemy.searchDuration;
        _changePointTimer = enemy.changePointTimer;
    }

    /// <summary>
    /// Проверяет, находится ли враг на точке поиска.
    /// </summary>
    private void OnThePoint(EnemyStateMachine enemy)
    {
        if (Vector3.Distance(enemy.transform.position, searchPoint) < enemy.offsetSearchinPoint)
        {
            PerformSearch(enemy);
        }
    }

    /// <summary>
    /// Проверяет, может ли враг добраться до точки поиска.
    /// </summary>
    private void CantGetToSearchPoint(EnemyStateMachine enemy)
    {
        if (_navMeshAgent.velocity.magnitude < 0.1f)
        {
            _timeToGet -= Time.deltaTime;
            if (_timeToGet <= 0)
            {
                BackToPatrol(enemy);
            }
        }
    }
}
