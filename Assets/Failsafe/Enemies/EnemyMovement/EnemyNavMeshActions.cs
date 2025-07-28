using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshActions
{
    private NavMeshAgent _navMeshAgent;
    private Transform _enemyPos;
    public EnemyNavMeshActions(NavMeshAgent navMeshAgent, Transform transform)
    {
        _navMeshAgent = navMeshAgent;
        _enemyPos =  transform;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }
    public void MoveToPoint(Vector3 point, float speed)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(point);
    }


    public void RunToPoint(Vector3 point, float speed)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(point);
    }

    public void StopMoving()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.speed = 0f;
    }

    public void ResumeMoving()
    {
        _navMeshAgent.isStopped = false;
    }
    
    public bool IsPointReached()
    {
        if (Vector3.Distance(_navMeshAgent.destination, _enemyPos.position) <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude < 0.05f)
            {
                StopMoving();
                return true;
            }
        }
        return false;
    }
    
    public void RotateToPoint(Vector3 targetPoint, float rotationSpeed = 5f)
    {
        Vector3 direction = targetPoint - _enemyPos.position;
        direction.y = 0f; // Игнорируем вертикаль (Y)

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _enemyPos.rotation = Quaternion.Slerp(_enemyPos.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    
}
