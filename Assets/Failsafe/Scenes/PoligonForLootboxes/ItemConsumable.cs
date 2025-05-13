using System.Linq;
using UnityEngine;

// https://docs.google.com/document/d/16ljHmcCaO7HHAfOLeaaGqA7uOBBWy8Pbcn7mU-EfDJo/edit?tab=t.1jwridjpc615
public class ItemConsumable : Item
{
    public enum ConsumableType
    {
        UNKNOWN,
        STEAMPACK,              // Редкость: Обычный
        ENERGY_BOOST,           // Редкость: Обычный
        ADRENALIN,              // Редкость: Редкий
        GORILLA_STIM,           // Редкость: Редкий
        TUSHKAN_STIM,           // Редкость: Редкий
        HLOPUSHKA,              // Редкость: Обычный
        HACK_KEY,               // Редкость: Редкий
        EMI_GRANADE,            // Редкость: Очень редкий
        DYNAMITE,               // Редкость: Очень редкий
        REMOTE_ACCESS_CONTROL,  // Редкость: Уникальный
        TELEKINEZ_GLOVES,       // Редкость: Уникальный
        STAZIS_PISTOL,          // Редкость: Очень редкий
        STEALTH_SYSTEM,         // Редкость: Очень редкий
        SUPER_ADRENALIN,        // Редкость: Очень редкий
        TELEPORT,               // Редкость: Уникальный (на один заход)
        ENERGY_WALL_GENERATOR,  // Редкость: Очень редкий
        COUNT, // Количество типа расходников
    };

    public enum ConsumableRarity
    {
        COMMON,
        RARE,
        VERY_RARE,
        UNIQUE
    };
    public ConsumableType Type { get; private set; } = ConsumableType.UNKNOWN;
    public ConsumableRarity Rarity { get; private set; } = ConsumableRarity.COMMON;

    void Start()
    {
        // Патроны либо аптечка
        // var type = Random.Range(1, 2);
        // SetType((RashidnikType)type);
        SetRarityColor();
    }

    public void SetType(ConsumableType type)
    {
        Type = type;
    }

    public void SetRarity(ConsumableRarity rarity)
    {
        Rarity = rarity;
        SetRarityColor();
    }

    public void SetRarityColor()
    {
        switch (Rarity)
        {
            case ConsumableRarity.COMMON:
                GetComponent<Renderer>().material.color = Color.gray;
                break;
            case ConsumableRarity.RARE:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case ConsumableRarity.VERY_RARE:
                GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case ConsumableRarity.UNIQUE:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }

    public void SetRandomTypeDependOnRarity(System.Random rnd, LootBoxInfo lootBoxInfo) // , int[] ConsumableCounter
    {
        switch (Rarity)
        {
            case ConsumableRarity.UNIQUE:
                {
                    // if (ConsumableCounter[(int)ConsumableType.TELEPORT] == 1)
                    if (lootBoxInfo.SpawnedTeleportConsumable)
                    {
                        var types = new ConsumableType[] {
                            ConsumableType.REMOTE_ACCESS_CONTROL,
                            ConsumableType.TELEKINEZ_GLOVES,
                        };
                        // выбираем остальное, так как телепорт выпадает в
                        // единичном экземпляре
                        // var rareTypes = GetRareTypes(ConsumableCounter, types);
                        Type = types[rnd.Next(0, types.Length)];
                    }
                    else
                    {
                        var types = new ConsumableType[] {
                            ConsumableType.REMOTE_ACCESS_CONTROL,
                            ConsumableType.TELEKINEZ_GLOVES,
                            ConsumableType.TELEPORT,
                        };
                        // var rareTypes = GetRareTypes(ConsumableCounter, types);
                        Type = types[rnd.Next(0, types.Length)];
                    }

                }
                break;
            case ConsumableRarity.VERY_RARE:
                {
                    var types = new ConsumableType[] {
                        ConsumableType.EMI_GRANADE,
                        ConsumableType.DYNAMITE,
                        ConsumableType.STAZIS_PISTOL,
                        ConsumableType.STEALTH_SYSTEM,
                        ConsumableType.SUPER_ADRENALIN,
                        ConsumableType.ENERGY_WALL_GENERATOR
                    };
                    // var rareTypes = GetRareTypes(ConsumableCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
            case ConsumableRarity.RARE:
                {
                    var types = new ConsumableType[] {
                        ConsumableType.ADRENALIN,
                        ConsumableType.GORILLA_STIM,
                        ConsumableType.TUSHKAN_STIM,
                        ConsumableType.HACK_KEY
                    };
                    // var rareTypes = GetRareTypes(ConsumableCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
            case ConsumableRarity.COMMON:
            default:
                {
                    var types = new ConsumableType[] {
                        ConsumableType.STEAMPACK,
                        ConsumableType.ENERGY_BOOST,
                        ConsumableType.HLOPUSHKA
                    };

                    // var rareTypes = GetRareTypes(ConsumableCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
        }
    }

    // private ConsumableType[] GetRareTypes(int[] ConsumableCounter, ConsumableType[] types)
    // {
    //     var rareTypes = new ConsumableType[] { };
    //     var lowest = int.MaxValue;
    //     for (int i = 0; i < types.Length; i++)
    //     {
    //         var counter = ConsumableCounter[(int)types[i]];
    //         if (counter < lowest)
    //         {
    //             lowest = counter;
    //             rareTypes = new ConsumableType[] { types[i] };
    //         }
    //         else if (counter == lowest)
    //         {
    //             rareTypes.Append(types[i]);
    //         }
    //     }

    //     return rareTypes;
    // }

    public string GetTypeName()
    {
        switch (Type)
        {
            case ConsumableType.STEAMPACK: return "Стимпак";
            case ConsumableType.ENERGY_BOOST: return "Батончик Energy Boost";
            case ConsumableType.ADRENALIN: return "Адреналин";
            case ConsumableType.GORILLA_STIM: return "Стимулятор Горилла";
            case ConsumableType.TUSHKAN_STIM: return "Стимулятор Тушкан";
            case ConsumableType.HLOPUSHKA: return "Хлопушка";
            case ConsumableType.HACK_KEY: return "Взлом ключ";
            case ConsumableType.EMI_GRANADE: return "ЭМИ Граната";
            case ConsumableType.DYNAMITE: return "Динамит";
            case ConsumableType.REMOTE_ACCESS_CONTROL: return "Пульт Удаленного Доступа";
            case ConsumableType.TELEKINEZ_GLOVES: return "Перчатка Телекинез";
            case ConsumableType.STAZIS_PISTOL: return "Стазис Пистолет";
            case ConsumableType.STEALTH_SYSTEM: return "Система Стелс";
            case ConsumableType.SUPER_ADRENALIN: return "Супер Адреналин";
            case ConsumableType.TELEPORT: return "Устройство Экстренной Телепортации";
            case ConsumableType.ENERGY_WALL_GENERATOR: return "Генератор Энерго Стен";
            case ConsumableType.UNKNOWN:
            case ConsumableType.COUNT:
            default:
                return "Неизвестно";
        }
    }

    override protected void Update()
    {
        base.Update();
    }

    public override bool PickUp()
    {
        bool check = base.PickUp();

        if (check)
        {
            Debug.Log($"Игрок подобрал расходник: {GetTypeName()}");
        }

        return check;
    }
}
