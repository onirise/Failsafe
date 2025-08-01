using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Item : Prop
{
    public ItemData ItemData;
    public List<ActionsGroup> ActionsGroups;
    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<BoxCollider>();
    }


    /// <summary>
    /// Предмет можно использовать
    /// </summary>
    /// <returns></returns>
    public bool IsUsable()
    {
        // Захардкоженное значение для Player/Use
        // Если вместо InputActionReference использовать простой enum то было бы проще с этим работать, пока такой костыль
        var playerUseActionId = System.Guid.Parse("316f217b-db19-4ab3-992d-f06d0052d966");
        return ActionsGroups.Where(x => x.Actions.Any(x => x.action.id == playerUseActionId)).Any();
    }

    /// <summary>
    /// Использовать предмет
    /// </summary>
    public void Use()
    {
        // Захардкоженное значение для Player/Use
        // Если вместо InputActionReference использовать простой enum то было бы проще с этим работать, пока такой костыль
        var playerUseActionId = System.Guid.Parse("316f217b-db19-4ab3-992d-f06d0052d966");
        foreach (var action in ActionsGroups.Where(x => x.Actions.Any(x => x.action.id == playerUseActionId)))
        {
            action.Invoke();
        }
    }

    /// <summary>
    /// Состояние в инвентаре/руке/ящике
    /// </summary>
    public void ToInventoryState()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }

    /// <summary>
    /// Состояние в игровом мире
    /// </summary>
    public void ToWorldState()
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
    }

    public void SetKinematic(bool value)
    {
        if (_rigidbody)
        {
            _rigidbody.isKinematic = value;
        }
    }
}