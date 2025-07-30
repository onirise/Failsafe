using DMDungeonGenerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class EnemyGetData
{
    private RoomData _currentRoom;
    private Transform _transform;
    public EnemyGetData(Transform transform)
    {
        _transform = transform;
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
    public void RoomCheck()
    {
        // Ищем все коллайдеры рядом с врагом
        Collider[] hits = Physics.OverlapSphere(_transform.position, 5f); 
        Debug.Log($"[Enemy] Обнаружено коллайдеров: {hits.Length}");

        foreach (var hit in hits)
        {
            Debug.Log($"[Enemy] Hit: {hit.name}");

            RoomData room = hit.GetComponentInChildren<RoomData>();
            if (room != null)
            {
                Debug.Log($"[Enemy] НАШЁЛ КОМНАТУ через OverlapSphere: {room.name}");
                SetCurrentRoom(room);
                break;
            }
        }

        // Получаем патрульные точки из установленной комнаты
        var points = GetRoomPatrolPoints();
        Debug.Log($"[Enemy] Получено точек патруля: {points.Count}");
    }
}