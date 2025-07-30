using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DirectionNodePair
{
    public Direction Direction;
    public PowerNode Node;
}

public enum Direction { Forward, Right, Back, Left }

public abstract class PowerNode : MonoBehaviour
{
    // Соседи по направлениям
    protected Dictionary<Direction, PowerNode> Neighbors;
    [SerializeField] protected List<DirectionNodePair> NeighborsSerialized = new List<DirectionNodePair>();

    // Какие направления соединены в этом узле (например, вход и выход)
    protected HashSet<Direction> ConnectedDirections;

    // Получил ли питание
    protected bool IsPowered = false;
    //public Direction FromDirection;
    protected bool PowerReceivedThisCycle = false;

    protected virtual void Awake()
    {
        Neighbors = new Dictionary <Direction, PowerNode>();
        ConnectedDirections = new HashSet<Direction>();
        foreach (var pair in NeighborsSerialized)
        {
            if (!Neighbors.ContainsKey(pair.Direction))
            {
                Neighbors.Add(pair.Direction, pair.Node);
                ConnectedDirections.Add(pair.Direction);
            }
        }
    }

    // Метод запуска распространения питания
    public void ReceivePower(Direction fromDirection)
    {
        if (IsPowered) return;
        Debug.Log(gameObject.name);
        if (!ConnectedDirections.Contains(Opposite(fromDirection)))
        {
            Debug.Log(ConnectedDirections.Contains(Opposite(fromDirection)));
            return; // Питание пришло не с подключенной стороны
        }
        Debug.Log(gameObject.name + "+");
        IsPowered = true;
        OnPowered();

        // Распространяем дальше по другим направлениям (кроме того, откуда пришли)
        foreach (var connectedDirection in ConnectedDirections)
        {
            if (connectedDirection == Opposite(fromDirection)) 
                continue;
            if (Neighbors.ContainsKey(connectedDirection))
            {
                Neighbors[connectedDirection].ReceivePower(connectedDirection);
            }
        }
    }

    // Метод для запуска питания снаружи (например, у начальной точки)
    public virtual void StartPower()
    {
        if (IsPowered) return;
        IsPowered = true;
        OnPowered();

        foreach (var connectedDirecrion in ConnectedDirections)
        {
            if (Neighbors.ContainsKey(connectedDirecrion))
            {
                Neighbors[connectedDirecrion].ReceivePower(connectedDirecrion);
            }
        }
    }

    protected virtual void OnPowered()
    {
        Debug.Log($"{name} powered");
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    // Метод для сброса питания (отключение)
    public virtual void ResetPower()
    {
        IsPowered = false;
        PowerReceivedThisCycle = false;
        OnPowerLost();
    }

    // Метод, вызываемый при отключении питания (можно переопределять)
    protected virtual void OnPowerLost()
    {
        Debug.Log($"{name} потеряно питание"); 
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public static Direction Opposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Direction.Back;
            case Direction.Back: return Direction.Forward;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        return dir;
    }
}
