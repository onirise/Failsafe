using System.Linq;
using UnityEngine;

public class ItemImplant : Item
{
    public enum ImplantType
    {
        UNKNOWN,    // Неизвестный – без названия
        ENHANCED_COMPLEX,   // Усиленный комплекс.      Редкость: Редкий
        REVOLVER_SYSTEM,    // Система револьвер.       Редкость: Очень редкий
        SOUND_VISUALIZER,   // Звуковой визуализатор.   Редкость: Редкий
        WAVE_SCANNER,       // Волновой сканер.         Редкость: Очень редкий
        EM_SCANNER,         // Электромагнитный сканер. Редкость: Редкий
        MODULE_ADMIN,       // Модуль «Администратор».  Редкость: Очень редкий
        COUNT,  // Кол-во
    };

    public enum ImplantRarity
    {
        RARE,
        VERY_RARE,
    };

    public ImplantType Type { get; private set; } = ImplantType.UNKNOWN;
    public ImplantRarity Rarity { get; private set; } = ImplantRarity.RARE;

    void Start()
    {
        SetRarityColor();
    }

    public void SetType(ImplantType type)
    {
        Type = type;
    }

    public void SetRarity(ImplantRarity rarity)
    {
        Rarity = rarity;
        SetRarityColor();
    }

    public void SetRarityColor()
    {
        switch (Rarity)
        {
            // case ImplantRarity.COMMON:
            //     GetComponent<Renderer>().material.color = Color.gray;
            //     break;
            case ImplantRarity.RARE:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case ImplantRarity.VERY_RARE:
                GetComponent<Renderer>().material.color = Color.magenta;
                break;
        }
    }

    public void SetRandomTypeDependOnRarity(System.Random rnd, int[] counter)
    {
        switch (Rarity)
        {
            case ImplantRarity.VERY_RARE:
                {
                    var types = new ImplantType[] {
                        ImplantType.REVOLVER_SYSTEM,
                        ImplantType.WAVE_SCANNER,
                        ImplantType.MODULE_ADMIN
                    };
                    var rareTypes = GetRareTypes(counter, types);
                    Type = rareTypes[rnd.Next(0, rareTypes.Length)];
                }
                break;
            case ImplantRarity.RARE:
            default:
                {
                    var types = new ImplantType[] {
                        ImplantType.ENHANCED_COMPLEX,
                        ImplantType.SOUND_VISUALIZER,
                        ImplantType.EM_SCANNER
                    };
                    var rareTypes = GetRareTypes(counter, types);
                    Type = rareTypes[rnd.Next(0, rareTypes.Length)];
                }
                break;
        }
    }

    // получение тех предметов из типов, которых еще не было
    // работает по принципу заранее полученного количества
    private ImplantType[] GetRareTypes(int[] counter, ImplantType[] types)
    {
        var rareTypes = new ImplantType[] { };
        var lowest = int.MaxValue;
        for (int i = 0; i < types.Length; i++)
        {
            var value = counter[(int)types[i]];
            if (value < lowest)
            {
                lowest = value;
                rareTypes = new ImplantType[] { types[i] };
            }
            else if (value == lowest)
            {
                rareTypes.Append(types[i]);
            }
        }

        return rareTypes;
    }

    public string GetTypeName()
    {
        switch (Type)
        {
            case ImplantType.ENHANCED_COMPLEX: return "Усиленный комплекс";
            case ImplantType.REVOLVER_SYSTEM: return "Система револьвер";
            case ImplantType.SOUND_VISUALIZER: return "Звуковой визуализатор";
            case ImplantType.WAVE_SCANNER: return "Волновой сканер";
            case ImplantType.EM_SCANNER: return "Электромагнитный сканер";
            case ImplantType.MODULE_ADMIN: return "Модуль «Администратор»";
            default: return "Неизвестно";
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
            Debug.Log($"Игрок подобрал имплант: {GetTypeName()}");
        return check;
    }
}
