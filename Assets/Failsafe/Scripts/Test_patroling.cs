using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Test_patroling : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWaypointIndex = 0;
    NavMeshAgent agent;
    float waitTimer = 2f;
    bool isWaiting = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].transform.position);
        }
    }

    private void Update()
    {
        if(isWaiting)
        {
            HandleWaiting();
        }
        else
        {
            CheckPatrolPointProximity();
        }

    }
    private void HandleWaiting()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            isWaiting = false;
            waitTimer = 2f; // Сброс таймера ожидания
            MoveToNextPatrolPoint();
        }
    }

    private void MoveToNextPatrolPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].transform.position);
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
}
