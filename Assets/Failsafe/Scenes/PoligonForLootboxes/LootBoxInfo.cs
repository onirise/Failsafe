using UnityEngine;
using static ItemRashodnik;

[CreateAssetMenu(fileName = "LootBoxInfo", menuName = "Scriptable Objects/LootBoxInfo")]
public class LootBoxInfo : ScriptableObject
{
    // Кол-во открытых лутбоксов с последнего дропнутого импланта
    public int OpenedLootBoxesSinceLastDroppedImplant = 0;

    // Кол-во лутбоксов, которые надо открыть чтобы получить имплант
    public int NeedToOpenLootBoxesToDropImaplant = 3;

    // Счетчик расходников (чего сколько выпало)
    // public int[] RashodnikCounter = new int[(int)RashodnikType.COUNT];

    // Счетчик имплантов (чего сколько выпало)
    public int[] ImplantCounter = new int[(int)ItemImplant.ImplantType.COUNT];

    public bool SpawnedTeleportRashodnik = false;

    public void Init()
    {
        OpenedLootBoxesSinceLastDroppedImplant = 0;
        // for (int i = 0; i < RashodnikCounter.Length; i++)
        // {
        //     RashodnikCounter[i] = 0;
        // }
        SpawnedTeleportRashodnik = false;

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

    // public void PrintRashodnikCounter() {
    //     string str = "Расходники: ";
    // }
}
