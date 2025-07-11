using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePoint : MonoBehaviour
{
    public int Value;
    [SerializeField] protected Text TextValue;
    public List<BasePoint> Connections = new List<BasePoint>();

    public virtual void OnEnable()
    {

    }

    public virtual void UpdateTextValue()
    {
        TextValue.text = Value.ToString();
    }

    public virtual int GetTotalValue()
    {
        return Value;
    }

    public virtual void Connect(BasePoint other)
    {
        if (!Connections.Contains(other))
            Connections.Add(other);
    }

    public virtual void Disconnect(BasePoint other)
    {
        if (Connections.Contains(other))
            Connections.Remove(other);
    }
    
}
