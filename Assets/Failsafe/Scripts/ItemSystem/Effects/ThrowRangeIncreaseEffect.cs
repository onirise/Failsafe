using UnityEngine;

public class ThrowRangeIncreaseEffect : BaseEffect
{
    public float ThrowRangeIncreaseAmount;
    public override void Apply()
    {
        //to-do Вызов увеличения Дальность броска персонажа
        Debug.Log($"Дальность броска персонажа изменена на {ThrowRangeIncreaseAmount}");
    }
}