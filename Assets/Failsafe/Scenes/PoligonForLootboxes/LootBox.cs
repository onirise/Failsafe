using System.Linq;
using UnityEngine;
using static ItemConsumable;

// https://docs.google.com/document/d/15ZrwKelBM5ZXM42MS_aEq3hCku1SUTj24FSYs0SaV0o/edit?tab=t.0
public class LootBox : MonoBehaviour
{
    [SerializeField] private LootBoxInfo lootBoxInfo;
    [SerializeField] private Item implantPrefab;
    [SerializeField] private Item ConsumablePrefab;
    private static System.Random rnd = null;
    private int itemCount = 0;
    public bool isOpened { get; private set; } = false;

    public int MinItemCount = 1;
    public int MaxItemCount = 5;

    void Start()
    {
        // окрашиваем для понимания, что лутбокс не открыт еще
        GetComponent<Renderer>().material.color = Color.green;

        if (rnd == null)
        {
            int seed = Random.Range(0, int.MaxValue);
            rnd = new System.Random(seed);
        }

        itemCount = rnd.Next(MinItemCount, MaxItemCount);
    }

    void Update()
    {

    }

    // Получения кол-во расходников, которые были у игрока
    // private int[] GetConsumableCounter()
    // {
    //     return lootBoxInfo.ConsumableCounter;
    // }

    // Кол-во имплантов, которые выпали из лутбокса
    private int[] GetImplantCounter()
    {
        return lootBoxInfo.ImplantCounter;
    }

    private void GenerateLoot()
    {


        var player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }
        // bool playerHasImplant = false;
        // var rashidnikCounts = GetConsumableCounts();

        // получение типа расходника, который меньше всего получали
        // var lowestConsumableIndex = 0;
        // var lowestConsumableCount = rashidnikCounts[0];
        // for (int i = 1; i < rashidnikCounts.Length; i++)
        // {
        //     if (rashidnikCounts[i] < lowestConsumableCount)
        //     {
        //         lowestConsumableCount = rashidnikCounts[i];
        //         lowestConsumableIndex = i;
        //     }
        // }

        // if (!playerHasImplant) { }

        for (int i = 0; i < itemCount; i++)
        {
            var rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            var top = Vector3.up * 1.0f;
            GameObject prefab = Instantiate(ConsumablePrefab.gameObject, transform.position + top, rotation);
            var rigidbody = prefab.GetComponent<Rigidbody>();
            var forcePower = 1.0f;
            var force = new Vector3(Random.Range(-1f, 1f) * forcePower, Random.Range(2.5f, 5f), Random.Range(-1f, 1f) * forcePower);
            rigidbody.AddForce(force, ForceMode.Impulse);

            // Задаем редкость расходнику
            var item = prefab.GetComponent<ItemConsumable>();
            var rarity = rnd.Next(100);
            if (rarity < 50)
            {
                item.SetRarity(ConsumableRarity.COMMON);
            }
            else if (rarity < 80)
            {
                item.SetRarity(ConsumableRarity.RARE);
            }
            else if (rarity < 95)
            {
                item.SetRarity(ConsumableRarity.VERY_RARE);
            }
            else
            {
                item.SetRarity(ConsumableRarity.UNIQUE);
            }

            // В зависимости от редкости задаем тип
            // NOTE: тип зависит от того, как часто выпадали различные расходники
            item.SetRandomTypeDependOnRarity(rnd, lootBoxInfo); // , GetConsumableCounter()

            if (item.Type == ConsumableType.TELEPORT)
            {
                lootBoxInfo.SpawnedTeleportConsumable = true;
            }
            // lootBoxInfo.ConsumableCounter[(int)item.Type]++;
        }

        lootBoxInfo.OpenedLootBoxesSinceLastDroppedImplant++;
        if (lootBoxInfo.OpenedLootBoxesSinceLastDroppedImplant >= lootBoxInfo.NeedToOpenLootBoxesToDropImaplant)
        {
            lootBoxInfo.OpenedLootBoxesSinceLastDroppedImplant %= lootBoxInfo.NeedToOpenLootBoxesToDropImaplant;

            var rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            var top = Vector3.up * 1.0f;
            GameObject prefab = Instantiate(implantPrefab.gameObject, transform.position + top, rotation);
            var rigidbody = prefab.GetComponent<Rigidbody>();
            var forcePower = 1.0f;
            var force = new Vector3(Random.Range(-1f, 1f) * forcePower, Random.Range(2.5f, 5f), Random.Range(-1f, 1f) * forcePower);
            rigidbody.AddForce(force, ForceMode.Impulse);

            // задаем редкость предмету
            var item = prefab.GetComponent<ItemImplant>();
            var rarity = rnd.Next(100);
            if (rarity < 50)
            {
                item.SetRarity(ItemImplant.ImplantRarity.RARE);
            }
            else
            {
                item.SetRarity(ItemImplant.ImplantRarity.VERY_RARE);
            }

            item.SetRandomTypeDependOnRarity(rnd, GetImplantCounter());
            lootBoxInfo.ImplantCounter[(int)item.Type]++;
        }
    }

    // Возвращает истину если лутбокс открылся
    // ложь если лутбокс уже открыт
    public bool Open()
    {
        if (isOpened) return false;
        // const string prefabPath = "Failsafe/Scenes/PoligonForLootboxes/Implant";
        // Item[] prefabs = 
        GenerateLoot(); // Resources.Load<Item>(prefabPath);

        // Создаем префаб над коробкой
        // if (prefabs.Length == 0)
        // {
        //     Debug.LogError("Item prefab not found in Resources folder.");
        //     return false;
        // }

        // foreach (var prefab in prefabs)
        // {
        //     var rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        //     var top = Vector3.up * 1.0f;
        //     GameObject implant = Instantiate(prefab.gameObject, transform.position + top, rotation);
        //     var rigidbody = implant.AddComponent<Rigidbody>();
        //     var forcePower = 3.0f;
        //     var force = new Vector3(Random.Range(-1f, 1f) * forcePower, Random.Range(5f, 10f), Random.Range(-1f, 1f) * forcePower);
        //     rigidbody.AddForce(force, ForceMode.Impulse);
        // }

        GetComponent<Renderer>().material.color = Color.red;
        isOpened = true;
        return true;
    }
}
