using UnityEngine;

public class HealEffect : BaseEffect
{
    public int HealAmount;
    public override void Apply()
    {
        //to-do Вызов хила для персонажа
        Debug.Log($"Персонаж восстановил {HealAmount}");
    }
}