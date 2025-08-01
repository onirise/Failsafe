using Failsafe.Items;
using Failsafe.Player.View;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Предмет в руке персонажа
/// </summary>
public class ItemInHand
{
    public Item ItemObject;
    public IUsable ItemUsable;
}


/// <summary>
/// Руки персонажа как контейнер предмета
/// </summary>
public class PlayerHandsContainer
{
    public enum HandState { EmptyHands, ItemInHand }

    public event Action OnItemTaked;
    public event Action OnItemDroped;
    public HandState State => _handState;
    public ItemInHand ItemInHand => _itemInHand;

    private ItemInHand _itemInHand;
    private HandState _handState = HandState.EmptyHands;
    private IEnumerable<IUsable> _items;
    private Transform _rightHandItemPlace;

    public PlayerHandsContainer(IEnumerable<IUsable> items, PlayerView playerView)
    {
        _items = items;
        _rightHandItemPlace = playerView.RightHandItemPlace;
    }

    /// <summary>
    /// Поместить предмет в руку
    /// </summary>
    /// <param name="itemObject">Предмет</param>
    /// <returns>true если предмет удалось взять в руку, иначе false</returns>
    public bool TryTakeItemInHand(Item itemObject)
    {
        if (_handState == HandState.ItemInHand)
        {
            Debug.Log("TryTakeItemInHand. Не получилось взять предмет. Руки заняты");
            return false;
        }
        // Сейчас скрипт предмета подбирается по названию предмета. 
        // Желательно сделать явный маппинг, например через общий enum в префабе и скрипте или через dictionary
        var usableItem = _items.FirstOrDefault(x => itemObject.name.StartsWith(x.GetType().Name));
        if (usableItem == null)
        {
            Debug.Log("TryTakeItemInHand. Не найден скрипт для предмета " + itemObject.name);
        }
        itemObject.ToInventoryState();
        itemObject.transform.SetParent(_rightHandItemPlace, false);
        itemObject.transform.localPosition = Vector3.zero;
        var itemInHand = new ItemInHand
        {
            ItemObject = itemObject,
            ItemUsable = usableItem
        };
        _itemInHand = itemInHand;
        _handState = HandState.ItemInHand;
        Debug.Log("Предмет взят в руку");
        OnItemTaked?.Invoke();
        return true;
    }

    /// <summary>
    /// Выбросить предмет из рук
    /// </summary>
    /// <returns></returns>
    public Item DropItemFromHand()
    {
        if (_handState == HandState.EmptyHands)
        {
            return null;
        }
        var item = _itemInHand.ItemObject;
        item.ToWorldState();
        _rightHandItemPlace.DetachChildren();
        _itemInHand = null;
        _handState = HandState.EmptyHands;
        OnItemDroped?.Invoke();
        return item;
    }

    /// <summary>
    /// Очистить руку
    /// </summary>
    public void SetItemNull()
    {
        _itemInHand = null;
        _handState = HandState.EmptyHands;
    }
}