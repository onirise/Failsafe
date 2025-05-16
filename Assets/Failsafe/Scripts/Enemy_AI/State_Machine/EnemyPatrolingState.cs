using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolingState : EnemyBaseState
{
    private NavMeshAgent agent;
    private GameObject[] patrolPoints;
    private int currentPatrolPointIndex = 0;
    private float waitTimer;
    private bool isWaiting = false;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;

    // Флаг для включения дебага
    private bool debugMode = false;

    /// <summary>
    /// Выполняется при входе в состояние патрулирования.
    /// </summary>
    public override void EnterState(EnemyStateMachine enemy)
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        patrolPoints = enemy.patrolPoints;

        if (agent == null || patrolPoints == null || patrolPoints.Length == 0)
        {
            if (debugMode) Debug.LogError($"[Patrol State] ({enemy.gameObject.name}) NavMeshAgent или патрульные точки не найдены!");
            return;
        }

        agent.speed = enemy.patrolSpeed;
        waitTimer = enemy.waitTime;
        lastPosition = agent.transform.position;
        stuckTimer = 0f;

        if (debugMode) Debug.Log($"[Patrol State] ({enemy.gameObject.name}) Начало патрулирования. Всего точек: {patrolPoints.Length}");
        MoveToNextPatrolPoint();
    }

    /// <summary>
    /// Выполняется при выходе из состояния патрулирования.
    /// </summary>
    public override void ExitState(EnemyStateMachine enemy)
    {
        agent.ResetPath();
        if (debugMode) Debug.Log($"[Patrol State] ({enemy.gameObject.name}) Выход из состояния патрулирования.");
    }

    /// <summary>
    /// Обновляет логику состояния патрулирования.
    /// </summary>
    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (agent == null || patrolPoints == null || patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            HandleWaiting(enemy);
        }
        else
        {
            CheckPatrolPointProximity(enemy);
        }

        // Проверка на застревание (если противник стоит на месте больше 2 секунд)
        if (Vector3.Distance(agent.transform.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2f)
            {
                if (debugMode) Debug.LogWarning($"[Patrol State] ({enemy.gameObject.name}) Противник застрял! Попытка перепрокладки маршрута.");
                MoveToNextPatrolPoint();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = agent.transform.position;
    }

    /// <summary>
    /// Обрабатывает ожидание на патрульной точке.
    /// </summary>
    private void HandleWaiting(EnemyStateMachine enemy)
    {
        waitTimer -= Time.deltaTime;
        if (debugMode) Debug.Log($"[Patrol State] ({enemy.gameObject.name}) Ожидание... Осталось времени: {waitTimer:F2}");

        if (waitTimer <= 0f)
        {
            isWaiting = false;
            waitTimer = enemy.waitTime;
            MoveToNextPatrolPoint();
        }
    }

    /// <summary>
    /// Проверяет расстояние до текущей патрульной точки.
    /// </summary>
    private void CheckPatrolPointProximity(EnemyStateMachine enemy)
    {
        if (!agent.pathPending)
        {
            float remainingDistance = agent.remainingDistance;

            if (debugMode) Debug.Log($"[Patrol State] ({enemy.gameObject.name}) Проверка дистанции до точки {currentPatrolPointIndex}. Осталось: {remainingDistance:F2}");

            if (remainingDistance <= agent.stoppingDistance + 0.1f && agent.hasPath)
            {
                if (debugMode) Debug.Log($"[Patrol State] ({enemy.gameObject.name}) Достигнута патрульная точка {currentPatrolPointIndex}");
                isWaiting = true;
            }
        }
    }

    /// <summary>
    /// Переключает врага на следующую патрульную точку.
    /// </summary>
    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        Vector3 targetPosition = patrolPoints[currentPatrolPointIndex].transform.position;

        if (agent.SetDestination(targetPosition))
        {
            if (debugMode) Debug.Log($"[Patrol State] ({agent.gameObject.name}) Переход к патрульной точке {currentPatrolPointIndex}: {targetPosition}");
        }
        else
        {
            if (debugMode) Debug.LogWarning($"[Patrol State] ({agent.gameObject.name}) Не удалось найти путь к точке {currentPatrolPointIndex}");
        }
    }
}


