using UnityEngine;
using System.Collections.Generic;

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

    public void SetKinematic(bool value)
    {
        var rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.isKinematic = value;
        }
    }
}