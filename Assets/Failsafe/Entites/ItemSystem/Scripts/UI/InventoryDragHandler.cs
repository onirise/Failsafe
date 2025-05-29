using UnityEngine;
using System.Linq;

/// <summary>
/// Реализует систему перетаскивания
/// Обрабатывает события UI для drag-and-drop
/// </summary>
public class InventoryDragHandler : MonoBehaviour
{
    [HideInInspector] public InventoryManager inventoryManager;
    [HideInInspector] public Camera playerCamera;

    [SerializeField] private LayerMask inventoryLayer;

    private Item draggedItem;
    private InventoryManager.InventorySlot originalSlot;

    private void Update()
    {
        if (!inventoryManager.IsInventoryActive) return;
        HandleDragInput();
    }

    public void SetDraggingItem(GameObject gameObject)
    {
        draggedItem = gameObject.GetComponent<Item>();
        if (draggedItem == null) return;
        originalSlot = null;
        draggedItem.SetKinematic(true);
    }

    private void HandleDragInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryEndDrag();
        }

        UpdateDragPosition();
    }

    private void TryStartDrag()
    {
        if (draggedItem)
        {
            return;
        }
        var (slot, item) = GetSlotAndItemUnderMouse();
        if (item != null)
        {
            draggedItem = item;
            originalSlot = slot;

            // Получаем позицию мыши в плоскости инвентаря
            Vector3 mousePos = GetMouseWorldPosition();
        }
    }

    public void TryEndDrag()
    {
        if (draggedItem == null) return;

        var (targetSlot, _) = GetSlotAndItemUnderMouse();
        if (inventoryManager.TryEndDragItem(originalSlot, targetSlot, draggedItem))
        {
            draggedItem = null;
            originalSlot = null;
        }
    }

    private void UpdateDragPosition()
    {
        if (draggedItem == null) return;

        // Получение позиции с учетом смещения
        Vector3 mousePos = GetMouseWorldPosition();

        // Плавное перемещение
        draggedItem.transform.position = Vector3.Lerp(
            draggedItem.transform.position,
            mousePos,
            Time.deltaTime * 20f
        );
    }

    private (InventoryManager.InventorySlot, Item) GetSlotAndItemUnderMouse()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100, inventoryLayer);

        foreach (var hit in hits.OrderBy(h => h.distance))
        {
            var slot = inventoryManager.GetSlotFromObject(hit.collider.gameObject);
            if (slot != null) return (slot, slot?.currentItem);
        }
        return (null, null);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        Plane inventoryPlane = new Plane(
            inventoryManager.transform.up, 
            inventoryManager.transform.position
        );

        if (inventoryPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}