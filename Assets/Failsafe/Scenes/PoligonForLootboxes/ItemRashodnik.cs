using System.Linq;
using UnityEngine;

// https://docs.google.com/document/d/16ljHmcCaO7HHAfOLeaaGqA7uOBBWy8Pbcn7mU-EfDJo/edit?tab=t.1jwridjpc615
public class ItemRashodnik : Item
{
    public enum RashodnikType
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

    public enum RashodnikRarity
    {
        COMMON,
        RARE,
        VERY_RARE,
        UNIQUE
    };
    public RashodnikType Type { get; private set; } = RashodnikType.UNKNOWN;
    public RashodnikRarity Rarity { get; private set; } = RashodnikRarity.COMMON;

    void Start()
    {
        // Патроны либо аптечка
        // var type = Random.Range(1, 2);
        // SetType((RashidnikType)type);
        SetRarityColor();
    }

    public void SetType(RashodnikType type)
    {
        Type = type;
    }

    public void SetRarity(RashodnikRarity rarity)
    {
        Rarity = rarity;
        SetRarityColor();
    }

    public void SetRarityColor()
    {
        switch (Rarity)
        {
            case RashodnikRarity.COMMON:
                GetComponent<Renderer>().material.color = Color.gray;
                break;
            case RashodnikRarity.RARE:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case RashodnikRarity.VERY_RARE:
                GetComponent<Renderer>().material.color = Color.magenta;
                break;
            case RashodnikRarity.UNIQUE:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
    }

    public void SetRandomTypeDependOnRarity(System.Random rnd, LootBoxInfo lootBoxInfo) // , int[] RashodnikCounter
    {
        switch (Rarity)
        {
            case RashodnikRarity.UNIQUE:
                {
                    // if (RashodnikCounter[(int)RashodnikType.TELEPORT] == 1)
                    if (lootBoxInfo.SpawnedTeleportRashodnik)
                    {
                        var types = new RashodnikType[] {
                            RashodnikType.REMOTE_ACCESS_CONTROL,
                            RashodnikType.TELEKINEZ_GLOVES,
                        };
                        // выбираем остальное, так как телепорт выпадает в
                        // единичном экземпляре
                        // var rareTypes = GetRareTypes(RashodnikCounter, types);
                        Type = types[rnd.Next(0, types.Length)];
                    }
                    else
                    {
                        var types = new RashodnikType[] {
                            RashodnikType.REMOTE_ACCESS_CONTROL,
                            RashodnikType.TELEKINEZ_GLOVES,
                            RashodnikType.TELEPORT,
                        };
                        // var rareTypes = GetRareTypes(RashodnikCounter, types);
                        Type = types[rnd.Next(0, types.Length)];
                    }

                }
                break;
            case RashodnikRarity.VERY_RARE:
                {
                    var types = new RashodnikType[] {
                        RashodnikType.EMI_GRANADE,
                        RashodnikType.DYNAMITE,
                        RashodnikType.STAZIS_PISTOL,
                        RashodnikType.STEALTH_SYSTEM,
                        RashodnikType.SUPER_ADRENALIN,
                        RashodnikType.ENERGY_WALL_GENERATOR
                    };
                    // var rareTypes = GetRareTypes(RashodnikCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
            case RashodnikRarity.RARE:
                {
                    var types = new RashodnikType[] {
                        RashodnikType.ADRENALIN,
                        RashodnikType.GORILLA_STIM,
                        RashodnikType.TUSHKAN_STIM,
                        RashodnikType.HACK_KEY
                    };
                    // var rareTypes = GetRareTypes(RashodnikCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
            case RashodnikRarity.COMMON:
            default:
                {
                    var types = new RashodnikType[] {
                        RashodnikType.STEAMPACK,
                        RashodnikType.ENERGY_BOOST,
                        RashodnikType.HLOPUSHKA
                    };

                    // var rareTypes = GetRareTypes(RashodnikCounter, types);
                    Type = types[rnd.Next(0, types.Length)];
                }
                break;
        }
    }

    // private RashodnikType[] GetRareTypes(int[] RashodnikCounter, RashodnikType[] types)
    // {
    //     var rareTypes = new RashodnikType[] { };
    //     var lowest = int.MaxValue;
    //     for (int i = 0; i < types.Length; i++)
    //     {
    //         var counter = RashodnikCounter[(int)types[i]];
    //         if (counter < lowest)
    //         {
    //             lowest = counter;
    //             rareTypes = new RashodnikType[] { types[i] };
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
            case RashodnikType.STEAMPACK: return "Стимпак";
            case RashodnikType.ENERGY_BOOST: return "Батончик Energy Boost";
            case RashodnikType.ADRENALIN: return "Адреналин";
            case RashodnikType.GORILLA_STIM: return "Стимулятор Горилла";
            case RashodnikType.TUSHKAN_STIM: return "Стимулятор Тушкан";
            case RashodnikType.HLOPUSHKA: return "Хлопушка";
            case RashodnikType.HACK_KEY: return "Взлом ключ";
            case RashodnikType.EMI_GRANADE: return "ЭМИ Граната";
            case RashodnikType.DYNAMITE: return "Динамит";
            case RashodnikType.REMOTE_ACCESS_CONTROL: return "Пульт Удаленного Доступа";
            case RashodnikType.TELEKINEZ_GLOVES: return "Перчатка Телекинез";
            case RashodnikType.STAZIS_PISTOL: return "Стазис Пистолет";
            case RashodnikType.STEALTH_SYSTEM: return "Система Стелс";
            case RashodnikType.SUPER_ADRENALIN: return "Супер Адреналин";
            case RashodnikType.TELEPORT: return "Устройство Экстренной Телепортации";
            case RashodnikType.ENERGY_WALL_GENERATOR: return "Генератор Энерго Стен";
            case RashodnikType.UNKNOWN:
            case RashodnikType.COUNT:
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
