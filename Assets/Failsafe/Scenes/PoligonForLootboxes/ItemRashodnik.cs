using System.Linq;
using UnityEngine;

// https://docs.google.com/document/d/16ljHmcCaO7HHAfOLeaaGqA7uOBBWy8Pbcn7mU-EfDJo/edit?tab=t.1jwridjpc615
public class ItemСonsumable : Item
{
    public enum СonsumableType
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

    public enum СonsumableRarity
    {
        COMMON,
        RARE,
        VERY_RARE,
        UNIQUE
    };
    public СonsumableType Type { get; private set; } = СonsumableType.UNKNOWN;
    public СonsumableRarity Rarity { get; private set; } = СonsumableRarity.COMMON;

    void Start()
    {
        // Патроны либо аптечка
        // var type = Random.Range(1, 2);
        // SetType((RashidnikType)type);
        SetRarityColor();
    }

    public void SetType(СonsumableType type)
    {
        Type = type;
    }

    public void SetRarity(СonsumableRarity rarity)
    {
        Rarity = rarity;
        SetRarityColor();
    }

    public void SetRarityColor()
    {
        switch (Rarity)
        {
            case СonsumableRarity.COMMON:
                GetComponent<Renderer>().material.color = Color.gray;
                break;
            case СonsumableRarity.RARE:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case СonsumableRarity.VERY_RARE:
                GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case СonsumableRarity.UNIQUE:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }

    public void SetRandomTypeDependOnRarity(System.Random rnd, LootBoxInfo lootBoxInfo) // , int[] СonsumableCounter
    {
        switch (Rarity)
        {
            case СonsumableRarity.UNIQUE:
                {
                    // if (СonsumableCounter[(int)СonsumableType.TELEPORT] == 1)
                    if (lootBoxInfo.SpawnedTeleportСonsumable)
                    {
                        var types = new СonsumableType[] {
                            СonsumableType.REMOTE_ACCESS_CONTROL,
                            СonsumableType.TELEKINEZ_GLOVES,
                        };
                        // выбираем остальное, так как телепорт выпадает в
                        // единичном экземпляре
                        // var rareTypes = GetRareTypes(СonsumableCounter, types);
                        Type = types[rnd.Next(0, types.Length)];
                    }
                    else
                    {
                        var types = new СonsumableType[] {
                            СonsumableType.REMOTE_ACCESS_CONTROL,
                            СonsumableType.TELEKINEZ_GLOVES,
                            СonsumableType.TELEPORT,
                        };
                        // var rareTypes = GetRareTypes(СonsumableCounter, types);
                        Type = types[rnd.Next(0, types.Length)];
                    }

                }
                break;
            case СonsumableRarity.VERY_RARE:
                {
                    var types = new СonsumableType[] {
                        СonsumableType.EMI_GRANADE,
                        СonsumableType.DYNAMITE,
                        СonsumableType.STAZIS_PISTOL,
                        СonsumableType.STEALTH_SYSTEM,
                        СonsumableType.SUPER_ADRENALIN,
                        СonsumableType.ENERGY_WALL_GENERATOR
                    };
                    // var rareTypes = GetRareTypes(СonsumableCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
            case СonsumableRarity.RARE:
                {
                    var types = new СonsumableType[] {
                        СonsumableType.ADRENALIN,
                        СonsumableType.GORILLA_STIM,
                        СonsumableType.TUSHKAN_STIM,
                        СonsumableType.HACK_KEY
                    };
                    // var rareTypes = GetRareTypes(СonsumableCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
            case СonsumableRarity.COMMON:
            default:
                {
                    var types = new СonsumableType[] {
                        СonsumableType.STEAMPACK,
                        СonsumableType.ENERGY_BOOST,
                        СonsumableType.HLOPUSHKA
                    };

                    // var rareTypes = GetRareTypes(СonsumableCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
        }
    }

    // private СonsumableType[] GetRareTypes(int[] СonsumableCounter, СonsumableType[] types)
    // {
    //     var rareTypes = new СonsumableType[] { };
    //     var lowest = int.MaxValue;
    //     for (int i = 0; i < types.Length; i++)
    //     {
    //         var counter = СonsumableCounter[(int)types[i]];
    //         if (counter < lowest)
    //         {
    //             lowest = counter;
    //             rareTypes = new СonsumableType[] { types[i] };
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
            case СonsumableType.STEAMPACK: return "Стимпак";
            case СonsumableType.ENERGY_BOOST: return "Батончик Energy Boost";
            case СonsumableType.ADRENALIN: return "Адреналин";
            case СonsumableType.GORILLA_STIM: return "Стимулятор Горилла";
            case СonsumableType.TUSHKAN_STIM: return "Стимулятор Тушкан";
            case СonsumableType.HLOPUSHKA: return "Хлопушка";
            case СonsumableType.HACK_KEY: return "Взлом ключ";
            case СonsumableType.EMI_GRANADE: return "ЭМИ Граната";
            case СonsumableType.DYNAMITE: return "Динамит";
            case СonsumableType.REMOTE_ACCESS_CONTROL: return "Пульт Удаленного Доступа";
            case СonsumableType.TELEKINEZ_GLOVES: return "Перчатка Телекинез";
            case СonsumableType.STAZIS_PISTOL: return "Стазис Пистолет";
            case СonsumableType.STEALTH_SYSTEM: return "Система Стелс";
            case СonsumableType.SUPER_ADRENALIN: return "Супер Адреналин";
            case СonsumableType.TELEPORT: return "Устройство Экстренной Телепортации";
            case СonsumableType.ENERGY_WALL_GENERATOR: return "Генератор Энерго Стен";
            case СonsumableType.UNKNOWN:
            case СonsumableType.COUNT:
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
