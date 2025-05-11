using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class ActionsGroup
{
    public InputActionReference ActionReference;
    public UnityEvent Event;

    public void Act() =>
        Event.Invoke();
}