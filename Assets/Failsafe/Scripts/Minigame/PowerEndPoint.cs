using UnityEngine;
using UnityEngine.Events;
// Конечная точка
public class PowerEndPoint : PowerNode
{
    public UnityEvent onPowered; // событие, которое вызывается при питании

    protected override void OnPowered()
    {
        base.OnPowered();
        Debug.Log($"{name} reached power!");
        onPowered?.Invoke();
    }
}
