using DMDungeonGenerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using UnityEngine.UIElements;

public class EnemyController
{
    private readonly Enemy _enemy;
    private readonly Transform _transform;
    private readonly NavMeshAgent _navMeshAgent;

    public RoomData _currentRoom { get; private set; }
    public EnemyController(Enemy enemy, Transform transform, NavMeshAgent navMeshAgent)
    {
        _enemy = enemy;
        _transform = transform;
        _navMeshAgent = navMeshAgent;
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
            Debug.LogWarning("GetRoomPatrolPoints: CurrentRoom is NULL");
            return new List<Transform>();
        }

        if (_currentRoom.PatrolPoints == null || _currentRoom.PatrolPoints.Count == 0)
        {
            Debug.LogWarning($"Комната {_currentRoom.name} имеет 0 точек — возможно, AutoCollectPatrolPoints() не вызван?");
        }

        return _currentRoom.GetPatrolPoints();
    }

    //Значения для скорости нужно будет брать из условного EnemyData, пока будет магические числа
    //TODO: интегрировать EnemyData
    public void MoveToPoint(Vector3 point)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = 5;
        _navMeshAgent.SetDestination(point);
    }
    public void RunToPoint(Vector3 point)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = 10;
        _navMeshAgent.SetDestination(point);
    }
    public void StopMoving()
    {
        _navMeshAgent.isStopped = true;
    }

    public void RotateToPoint(Vector3 point, Vector3 worldUp)
    {
        Vector3 direction = point - _enemy.transform.position;
        direction.y = 0f; // игнор высоты

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, worldUp);
        _enemy.transform.rotation = Quaternion.Slerp(
            _enemy.transform.rotation,
            targetRotation,
            Time.deltaTime * 5f // можно вынести скорость в поле
        );
    }

    public bool IsPointReached()
    {
        if (Vector3.Distance(_navMeshAgent.destination, _navMeshAgent.transform.position) <= 3f)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                Debug.Log("Reached destination");
                return true;
            }
        }
        return false;
    }

    public Vector3 RandomPoint()
    {
        Vector3 nextPatrolPoint = new Vector3(
            UnityEngine.Random.Range(_transform.position.x - 10,
            _transform.position.x + 10),
            0,
            UnityEngine.Random.Range(_transform.position.z - 10,
            _transform.position.z + 10)
        );

        return nextPatrolPoint;
    }
}
