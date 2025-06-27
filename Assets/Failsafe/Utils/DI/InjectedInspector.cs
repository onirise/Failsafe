#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class InjectedInspector : SerializedMonoBehaviour
{
    private IObjectResolver _resolver;

    [OdinSerialize, ValueDropdown(nameof(GetInterfaceTypes))]
    public List<Type> InterfaceTypes = new List<Type>();

    [ShowInInspector, ShowIf("@UnityEngine.Application.isPlaying")]
    public List<object> InjectedInstances = new List<object>();

    private Type[] GetInterfaceTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsInterface)
            .ToArray();
    }

    [Inject]
    private void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
        InjectAll();
    }

    [InfoBox("Injection must work automatically. This button is for runtime changes.")]
    [Button("Inject All"), ShowIf("@UnityEngine.Application.isPlaying")]
    public void InjectAll()
    {
        InjectedInstances.Clear();

        foreach (var type in InterfaceTypes)
        {
            if (type == null)
            {
                Debug.LogWarning($"Interface is null");
                continue;
            }

            try
            {
                var instance = _resolver.Resolve(type);
                InjectedInstances.Add(instance);
                Debug.Log($"Injected {type.Name}: {instance != null}");
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to inject {type.Name}: {exception.Message}");
                InjectedInstances.Add(null);
            }
        }
    }

    [Button("Clear All"), ShowIf("@UnityEngine.Application.isPlaying")]
    public void ClearAll()
    {
        InterfaceTypes.Clear();
        InjectedInstances.Clear();
    }
}
#endif