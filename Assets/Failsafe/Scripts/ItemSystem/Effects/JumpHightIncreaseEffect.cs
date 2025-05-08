using UnityEngine;

public class JumpHightIncreaseEffect : BaseEffect
{
    public float JumpHightIncreaseAmount;
    public override void Apply()
    {
        //to-do Вызов увеличения Высоты прыжка
        Debug.Log($"Высота прыжка персонажа изменена на {JumpHightIncreaseAmount}");
    }
}