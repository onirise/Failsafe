using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class ActionsGroup
{
    public List<InputActionReference> Actions;
    public UnityEvent Event;

    public void Invoke() =>
        Event.Invoke();
}