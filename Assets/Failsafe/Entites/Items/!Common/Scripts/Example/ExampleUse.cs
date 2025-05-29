using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.InputSystem;
using System.Linq;

public class ExampleUse : MonoBehaviour
{
    [SerializeField] private InputActionReference _action;

    [Button]
    public void Invoke()
    {
        var items = GetComponentsInChildren<Item>();
        items.ForEach(item => item.ActionsGroups.Where(x => x.Actions.Contains(_action)).ForEach(y => y.Invoke()));
    }
}
