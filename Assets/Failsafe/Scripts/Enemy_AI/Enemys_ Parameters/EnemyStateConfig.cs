using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStateConfig", menuName = "AI/Enemy State Config")]
public class EnemyStateConfig : ScriptableObject
{
    [SerializeField]
    [Tooltip("Список всех доступных состояний для данного типа противника")]
    public List<string> availableStates = new List<string>();

    public List<string> AvailableStates => availableStates;

    // Автоматически находит все состояния, наследуемые от EnemyBaseState
    public void RefreshStates()
    {
        availableStates.Clear();
        var types = GetAllEnemyStateTypes();
        foreach (var type in types)
        {
            availableStates.Add(type.FullName);
        }
    }

    // Метод поиска всех классов, наследуемых от EnemyBaseState
    private static IEnumerable<Type> GetAllEnemyStateTypes()
    {
        var assembly = typeof(EnemyBaseState).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(EnemyBaseState)) && !type.IsAbstract)
            {
                yield return type;
            }
        }
    }
}