using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет структурой сетки
/// Контролирует состояние слотов
/// Обрабатывает добавление/удаление предметов
/// Управляет подсветкой
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int gridWidth = 6;
    [SerializeField] private int gridHeight = 3;
    [SerializeField] public float gridSpacing = 0.2f;
    [SerializeField] public GameObject slotPrefab;

    [Header("Testing")]
    [SerializeField] private bool addTestItemOnStart = true;
    [SerializeField] public GameObject [] itemPrefabs;
    public bool IsInventoryActive { get; private set; }

    private InventorySlot[,] slots;

    [System.Serializable]
    public class InventorySlot
    {
        public GameObject slotObject;
        public Item currentItem;
        public BoxCollider slotCollider; 
    }
    public void SetInventoryState(bool state)
    {
        IsInventoryActive = state;
    }
    public bool TryEndDragItem(InventorySlot fromSlot, InventorySlot toSlot, Item item)
    {
        if (toSlot == null)
        {
            item.transform.SetParent(null);
            if(fromSlot is not null) fromSlot.currentItem = null;
            item.SetKinematic(false);
            SetLayerRecursively(item.gameObject, LayerMask.NameToLayer("Default"));
            return true;
        }

        if (toSlot.currentItem == null)
        {
            if (fromSlot is not null)
            {
                fromSlot.currentItem = null;
            }
            else
            {
                CreateItemInSlot(toSlot, item);
            }
            toSlot.currentItem = item;
            item.transform.position = toSlot.slotObject.transform.position;
            item.transform.SetParent(toSlot.slotObject.transform);
        }
        else if (fromSlot is not null && fromSlot.currentItem != null)
        {
            SwapItems(fromSlot, toSlot);
        } 
        else
        {
            return false;
        }
        return true;
    }
    public void AddItemToRandomSlot(GameObject item)
    {
        // Ищем случайный свободный слот
        List<InventorySlot> emptySlots = new List<InventorySlot>();
        foreach (var slot in slots)
        {
            if (slot.currentItem == null)
            {
                emptySlots.Add(slot);
            }
        }

        if (emptySlots.Count > 0)
        {
            int randomIndex = Random.Range(0, emptySlots.Count);
            CreateItemInSlot(emptySlots[randomIndex], item);
        }
    }
    public InventorySlot GetSlotFromObject(GameObject slotObject)
    {
        foreach (var slot in slots)
        {
            if (slot.slotObject == slotObject)
            {
                return slot;
            }
        }
        return null;
    }
    public Item GetItemFromBelt(int slotNum)
    {
        return slots[gridHeight - 1, slotNum - 1].currentItem;
    }
    private void Start()
    {
        InitializeSlots();
        GenerateGrid();

        if (addTestItemOnStart)
        {
            AddTestItems();
        }
    }
    private void CreateItemInSlot(InventorySlot slot, GameObject item)
    {
        if (itemPrefabs.Length == 0) return;

        GameObject newItemObj = Instantiate(item, slot.slotObject.transform);
        SetLayerRecursively(newItemObj, LayerMask.NameToLayer("Inventory")); 

        Item newItem = newItemObj.GetComponent<Item>();
        if (!newItem) newItem = newItemObj.AddComponent<Item>();

        newItem.SetKinematic(true);

        // Масштабирование под размер слота
        FitItemToSlot(newItemObj.transform, slot.slotCollider);

        slot.currentItem = newItem;
    }
    private void CreateItemInSlot(InventorySlot slot, Item item)
    {
        if (itemPrefabs.Length == 0) return;

        SetLayerRecursively(item.gameObject, LayerMask.NameToLayer("Inventory"));
        item.SetKinematic(true);
        // Масштабирование под размер слота
        //FitItemToSlot(item.transform, slot.slotCollider);
        slot.currentItem = item;
    }
    private void SetLayerRecursively(GameObject parentObj, int layer)
    {
        if (parentObj == null) return;

        parentObj.layer = layer;

        foreach (Transform child in parentObj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
    private void FitItemToSlot(Transform item, BoxCollider slotCollider)
    {
        BoxCollider itemCollider = item.GetComponent<BoxCollider>();
        if (!itemCollider || !slotCollider) return;

        // Вычисляем масштаб
        Vector3 slotSize = slotCollider.size;
        Vector3 itemSize = itemCollider.size;

        float scaleFactor = Mathf.Min(
            slotSize.x / itemSize.x,
            slotSize.y / itemSize.y,
            slotSize.z / itemSize.z
        );

        item.localScale = Vector3.one * scaleFactor * 0.95f; // 5% отступ
        item.localPosition = Vector3.zero;
    }
    private void AddTestItems()
    {
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            AddItemToRandomSlot(itemPrefabs[i]);
        }
    }
    private void InitializeSlots()
    {
        slots = new InventorySlot[gridHeight, gridWidth];
    }
    private void GenerateGrid()
    {
        Vector3 startposition = new Vector3(
                    0 - gridWidth/2 * gridSpacing,
                    0,
                    0
                ); 
        for (int row = 0; row < gridHeight; row++)
        {
            for (int col = 0; col < gridWidth; col++)
            {
                Vector3 pos = new Vector3(
                    startposition.x + col * gridSpacing,
                    startposition.y ,
                    startposition.z + row * gridSpacing
                );

                GameObject slot = Instantiate(slotPrefab, transform);
                slot.transform.localPosition = pos;
                slot.layer = LayerMask.NameToLayer("Inventory");


                // Инициализация коллайдера слота
                slots[row, col] = new InventorySlot
                {
                    slotObject = slot,
                    currentItem = null,
                    slotCollider = slot.GetComponent<BoxCollider>()
                };
            }
        }
    }
    private void SwapItems(InventorySlot a, InventorySlot b)
    {
        // Меняем местами
        var tempItemA = a.currentItem;
        var tempItemB = b.currentItem;

        if (tempItemA != null)
        {
            tempItemA.transform.SetParent(b.slotObject.transform);
            tempItemA.transform.localPosition = Vector3.zero;
        }

        if (tempItemB != null)
        {
            tempItemB.transform.SetParent(a.slotObject.transform);
            tempItemB.transform.localPosition = Vector3.zero;
        }

        // Обновляем ссылки
        a.currentItem = tempItemB;
        b.currentItem = tempItemA;
    }
}