using Sirenix.Utilities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class Item : Prop
{
    public ItemData ItemData;
    public List<ActionsGroup> ActionsGroups;

    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    public void Use()
    {
        ActionsGroups.ForEach(y => y.Invoke());
    }
    public void Use(InputActionReference action)
    {
        ActionsGroups.Where(x => x.Actions.Contains(action)).ForEach(y => y.Invoke());
    }

    public void SetKinematic(bool value)
    {
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.isKinematic = value;
        }
    }
}