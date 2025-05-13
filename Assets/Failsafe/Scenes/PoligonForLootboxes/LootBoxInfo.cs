using UnityEngine;
using static ItemСonsumable;

[CreateAssetMenu(fileName = "LootBoxInfo", menuName = "Scriptable Objects/LootBoxInfo")]
public class LootBoxInfo : ScriptableObject
{
    // Кол-во открытых лутбоксов с последнего дропнутого импланта
    public int OpenedLootBoxesSinceLastDroppedImplant = 0;

    // Кол-во лутбоксов, которые надо открыть чтобы получить имплант
    public int NeedToOpenLootBoxesToDropImaplant = 3;

    // Счетчик расходников (чего сколько выпало)
    // public int[] СonsumableCounter = new int[(int)СonsumableType.COUNT];

    // Счетчик имплантов (чего сколько выпало)
    public int[] ImplantCounter = new int[(int)ItemImplant.ImplantType.COUNT];

    public bool SpawnedTeleportСonsumable = false;

    public void Init()
    {
        OpenedLootBoxesSinceLastDroppedImplant = 0;
        // for (int i = 0; i < СonsumableCounter.Length; i++)
        // {
        //     СonsumableCounter[i] = 0;
        // }
        SpawnedTeleportСonsumable = false;

        for (int i = 0; i < ImplantCounter.Length; i++)
        {
            ImplantCounter[i] = 0;
        }
    }

    private void Awake()
    {
        // Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Reset()
    {

    }

    // public void PrintСonsumableCounter() {
    //     string str = "Расходники: ";
    // }
}
