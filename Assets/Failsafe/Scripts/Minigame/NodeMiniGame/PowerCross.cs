using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Перекрёсток (2-4 направления, можно поворачивать)

public class PowerCross : PowerNode
{
    // Ориентация - поворот на 90 градусы
    private int _rotationSteps = 0; // 0..3

    [SerializeField] private Direction[] _baseConnections = new Direction[] { 
        Direction.Forward, 
        Direction.Left}; // пример базовой конфигурации

    protected override void Awake()
    {
        Neighbors = new Dictionary<Direction, PowerNode>();
        ConnectedDirections = new HashSet<Direction>();
        foreach (var pair in NeighborsSerialized)
        {
            if (!Neighbors.ContainsKey(pair.Direction))
            {
                Neighbors.Add(pair.Direction, pair.Node);
            }
        }
        UpdateConnectedDirections();
    }
    public void Rotate()
    {
        _rotationSteps = (_rotationSteps + 1) % 4;
        transform.localRotation *= Quaternion.AngleAxis(90f, Vector3.up);
        UpdateConnectedDirections();
        // Найдём менеджер и попросим обновить питание
        var manager =  FindFirstObjectByType<PowerNetworkManager>();
        if (manager != null)
        {
            manager.RefreshPower();
        }
        else
        {
            Debug.LogWarning("PowerNetworkManager not found in scene.");
        }
    }
    private void UpdateConnectedDirections()
    {
        ConnectedDirections.Clear();
        foreach (var baseConnection in _baseConnections)
        {
            ConnectedDirections.Add(RotateDirection(baseConnection, _rotationSteps));
        }
    }

    private Direction RotateDirection(Direction connection, int steps)
    {
        int intConnection = (int)connection;
        intConnection = (intConnection + steps) % 4;
        Debug.Log((Direction)intConnection);
        return (Direction)intConnection;
    }

}
