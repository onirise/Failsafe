using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class Item : Prop
{
    public ItemData ItemData;
    public List<ActionsGroup> ActionsGroups;
    
    public void SetKinematic(bool value)
    {
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.isKinematic = value;
        }
    }
}