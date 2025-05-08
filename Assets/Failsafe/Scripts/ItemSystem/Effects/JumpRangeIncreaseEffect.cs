using UnityEngine;

public class JumpRangeIncreaseEffect : BaseEffect
{
    public float JumpRangeIncreaseAmount;
    public override void Apply()
    {
        //to-do Вызов увеличения Дальности прыжка
        Debug.Log($"Дальность прыжка персонажа изменена на {JumpRangeIncreaseAmount}");
    }
}