using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolingState : EnemyBaseState
{
    private NavMeshAgent agent;
    private GameObject[] patrolPoints;
    private int currentPatrolPointIndex = 0;
    private float waitTimer;
    private bool isWaiting = false;

    /// <summary>
    /// Выполняется при входе в состояние патрулирования.
    /// </summary>
    public override void EnterState(EnemyStateMachine enemy)
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        patrolPoints = enemy.patrolPoints;

        if (agent == null || patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("NavMeshAgent или патрульные точки не найдены!");
            return;
        }

        agent.speed = enemy.patrolSpeed;
        waitTimer = enemy.waitTime;
        MoveToNextPatrolPoint();
    }

    /// <summary>
    /// Выполняется при выходе из состояния патрулирования.
    /// </summary>
    public override void ExitState(EnemyStateMachine enemy)
    {
        agent.ResetPath(); // Сброс пути
        Debug.Log("Exiting Patrol State");
    }

    /// <summary>
    /// Обновляет логику состояния патрулирования.
    /// </summary>
    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (isWaiting)
        {
            HandleWaiting(enemy);
        }
        else
        {
            CheckPatrolPointProximity();
        }

        enemy.LookForPlayer();
        enemy.CheckForPlayer(enemy);
    }

    /// <summary>
    /// Обрабатывает ожидание на патрульной точке.
    /// </summary>
    private void HandleWaiting(EnemyStateMachine enemy)
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            isWaiting = false;
            waitTimer = enemy.waitTime;
            MoveToNextPatrolPoint();
        }
    }

    /// <summary>
    /// Проверяет расстояние до текущей патрульной точки.
    /// </summary>
    private void CheckPatrolPointProximity()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            isWaiting = true;
        }
    }

    /// <summary>
    /// Переключает врага на следующую патрульную точку.
    /// </summary>
    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolPointIndex].transform.position);
    }
}
