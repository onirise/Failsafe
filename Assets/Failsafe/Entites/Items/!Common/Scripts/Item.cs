using UnityEngine;

public class Item : Prop
{
    public ItemData ItemData;
    public ActionsGroupList ActionsGroups;

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