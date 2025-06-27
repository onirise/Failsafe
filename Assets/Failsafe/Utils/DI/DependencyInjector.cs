using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Reflection;

public class DependencyInjector : MonoBehaviour
{
    private IObjectResolver _resolver;

    [Inject]
    private void Construct(IObjectResolver resolver)
    {
        _resolver = resolver;
    }

    public void InjectDependencies(GameObject target)
    {
        _resolver.InjectGameObject(target);
    }

    public void ClearDependencies(GameObject target)
    {
        var components = target.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var component in components)
        {
            ClearInterfaceDependencies(component);
        }
    }

    private void ClearInterfaceDependencies(MonoBehaviour component)
    {
        if (component == null) return;

        var fields = component.GetType().GetFields(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.FieldType.IsInterface)
            {
                field.SetValue(component, null);
            }
        }
    }
}