using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStateFactory
{
    // Словарь пулов для каждого типа состояния
    private static readonly Dictionary<Type, Queue<EnemyBaseState>> _statePools = new();

    // Словарь доступных состояний для каждого противника
    private static readonly Dictionary<int, List<Type>> _enemyStates = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeFactory()
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log("EnemyStateFactory: Инициализировано!");
        }
    }

    /// <summary>
    /// Регистрация состояния по Type
    /// </summary>
    public static void RegisterState(Type stateType)
    {
        if (!stateType.IsSubclassOf(typeof(EnemyBaseState)))
        {
            Debug.LogWarning($"Тип {stateType.Name} не является наследником EnemyBaseState и не может быть зарегистрирован.");
            return;
        }

        if (!_statePools.ContainsKey(stateType))
        {
            _statePools[stateType] = new Queue<EnemyBaseState>();
            if (Debug.isDebugBuild)
            {
                Debug.Log($"[EnemyStateFactory] Зарегистрировано состояние: {stateType.Name}");
            }
        }
    }

    /// <summary>
    /// Регистрация доступных состояний для конкретного противника
    /// </summary>
    public static void RegisterEnemyStates(int enemyId, List<Type> availableStates)
    {
        if (!_enemyStates.ContainsKey(enemyId))
        {
            _enemyStates[enemyId] = new List<Type>();
        }

        foreach (var state in availableStates)
        {
            if (!_enemyStates[enemyId].Contains(state))
            {
                _enemyStates[enemyId].Add(state);
                RegisterState(state); // Регистрация в общем пуле
            }
        }

        if (Debug.isDebugBuild)
        {
            Debug.Log($"[EnemyStateFactory] Для противника {enemyId} зарегистрированы состояния: " +
                      $"{string.Join(", ", _enemyStates[enemyId])}");
        }
    }

    /// <summary>
    /// Получение состояния из пула или создание нового
    /// </summary>
    public static EnemyBaseState GetState(string stateName, int enemyId)
    {
        Type stateType = _enemyStates[enemyId].Find(type => type.Name == stateName);

        if (stateType == null)
        {
            Debug.LogError($"[EnemyStateFactory] Состояние '{stateName}' не зарегистрировано для противника {enemyId}");
            return null;
        }

        if (_statePools.TryGetValue(stateType, out var pool) && pool.Count > 0)
        {
            if (Debug.isDebugBuild)
                Debug.Log($"[EnemyStateFactory] Взято из пула: {stateType.Name}");

            return pool.Dequeue();
        }

        if (Debug.isDebugBuild)
            Debug.Log($"[EnemyStateFactory] Создано новое состояние: {stateType.Name}");

        return Activator.CreateInstance(stateType) as EnemyBaseState;
    }

    /// <summary>
    /// Возвращение состояния в пул
    /// </summary>
    public static void ReturnState(EnemyBaseState state)
    {
        var type = state.GetType();

        if (!_statePools.ContainsKey(type))
        {
            Debug.LogWarning($"Попытка вернуть в пул незарегистрированное состояние: {type.Name}");
            return;
        }

        if (Debug.isDebugBuild)
            Debug.Log($"[EnemyStateFactory] Возвращено в пул: {type.Name}");

        _statePools[type].Enqueue(state);
    }
}