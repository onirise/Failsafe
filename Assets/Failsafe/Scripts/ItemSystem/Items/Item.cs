using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс для всех предметов
/// Управляет визуальной подсветкой
/// Содержит логику предмета
/// </summary>
public class Item : MonoBehaviour
{
    //Я бы как сделал: Есть предметы (в т.ч. пропы), некоторые из них можно волочить, некоторые взять в руки, некоторые ещё и убрать в инвентарь. Зависит от размеров (https://docs.google.com/document/d/1sTkx0TCpQNYyGT-ZiLcsQcQ3usvauy_Qi0h-b4-ZqNM/edit?tab=t.0#heading=h.2slexxz8qzx)
    //Предметы, которые можно положить в инвентарь можно из него вынуть и взять в руки. Некоторые предметы при выборе их в инвентаре можно сразу использовать (без вынимания)
    //Все предметы, которые могут лежать в руках можно кинуть/положить (в т.ч те, которые сразу используются)

    //Архитектура:
    //enum для размера предмета
    //Пусть будет абстрактный класс Item: что-то, что можно взять в руку и положить в инвентарь
    //Интерфейс IUsable: туда переместить Use

    public ItemRarity Rarity;
    public ItemSize Size;

    private void Awake()
    {
        if (!GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }
}