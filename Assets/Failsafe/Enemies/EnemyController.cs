using DMDungeonGenerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class EnemyController
{
    private readonly Transform _transform;
    private readonly NavMeshAgent _navMeshAgent;
    private RoomData _currentRoom;

    public EnemyController(Transform transform, NavMeshAgent navMeshAgent)
    {
        _transform = transform;
        _navMeshAgent = navMeshAgent;

        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    public void MoveToPoint(Vector3 point)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = 3f;
        _navMeshAgent.SetDestination(point);
    }


    public void RunToPoint(Vector3 point)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = 7f;
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

    public void SetCurrentRoom(RoomData room)
    {
        _currentRoom = room;
    }

    public RoomData CurrentRoom => _currentRoom;

    public List<Transform> GetRoomPatrolPoints()
    {
        if (_currentRoom == null)
        {
            Debug.LogWarning("CurrentRoom is NULL");
            return new List<Transform>();
        }

        if (_currentRoom.PatrolPoints == null || _currentRoom.PatrolPoints.Count == 0)
        {
            Debug.LogWarning($"Комната {_currentRoom.name} не содержит PatrolPoints — AutoCollectPatrolPoints?");
        }

        return _currentRoom.GetPatrolPoints();
    }

    public bool IsPointReached()
    {
        if (Vector3.Distance(_navMeshAgent.destination, _transform.position) <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude < 0.05f)
            {
                StopMoving();
                return true;
            }
        }
        return false;
    }

    public Vector3 RandomPoint()
    {
        return new Vector3(
            UnityEngine.Random.Range(_transform.position.x - 10, _transform.position.x + 10),
            0,
            UnityEngine.Random.Range(_transform.position.z - 10, _transform.position.z + 10)
        );
    }

    public void RotateToPoint(Vector3 targetPoint, float rotationSpeed = 5f)
    {
        Vector3 direction = targetPoint - _transform.position;
        direction.y = 0f; // Игнорируем вертикаль (Y)

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}