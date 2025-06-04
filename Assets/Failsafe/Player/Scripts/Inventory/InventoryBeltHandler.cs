using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Реализует систему перетаскивани¤
/// Обрабатывает события UI для drag-and-drop
/// </summary>
public class InventoryBeltHandler : MonoBehaviour
{
    [HideInInspector] public InventoryManager inventoryManager;
    private void Update()
    {
        HandleQuickPanel();
    }
    private void HandleQuickPanel()
    {
        //if (Input.GetKeyUp(KeyCode.Alpha1))
        //{
        //    var item = inventoryManager.GetItemFromBelt(1);
        //    item.Use();
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha2))
        //{
        //    var item = inventoryManager.GetItemFromBelt(2);
        //    item.Use();
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha3))
        //{
        //    var item = inventoryManager.GetItemFromBelt(3);
        //    item.Use();
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha4))
        //{
        //    var item = inventoryManager.GetItemFromBelt(4);
        //    item.Use();
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha5))
        //{
        //    var item = inventoryManager.GetItemFromBelt(5);
        //    item.Use();
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha6))
        //{
        //    var item = inventoryManager.GetItemFromBelt(6);
        //    item.Use();
        //}
    }
}