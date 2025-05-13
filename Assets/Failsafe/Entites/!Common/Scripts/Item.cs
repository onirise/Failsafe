using System.Collections.Generic;
using UnityEngine;

public class Item : Prop
{
    public ItemData ItemData;
    public List<ActionsGroup> ActionsGroups = new List<ActionsGroup>();
    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
}