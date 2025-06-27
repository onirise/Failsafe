using UnityEngine;
using Sirenix.OdinInspector;
using Failsafe.Scripts.Health;
using Failsafe.Player.Model;
using System.Collections.Generic;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "PlayerDependenciesConfig", menuName = "Player/PlayerDependenciesConfig")]
public class PlayerDependenciesConfig : SerializedScriptableObject
{
    [Required, ValueDropdown(nameof(GetIHealthTypes))]
    public Type HealthType;

    [Required, ValueDropdown(nameof(GetIStaminaTypes))]
    public Type StaminaType;

    private IEnumerable<Type> GetIHealthTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IHealth).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface);
    }

    private IEnumerable<Type> GetIStaminaTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IStamina).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface);
    }
}
