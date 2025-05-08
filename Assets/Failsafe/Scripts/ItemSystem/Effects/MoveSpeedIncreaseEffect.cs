using UnityEngine;

public class MoveSpeedIncreaseEffect : BaseEffect
{
    public float MoveSpeedIncreaseAmount;
    public override void Apply()
    {
        //to-do Вызов увеличения скорости персонажа
        Debug.Log($"Скорость персонажа изменена на {MoveSpeedIncreaseAmount}");
    }
}