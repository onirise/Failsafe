using UnityEngine;

public class HealthIncreaseEffect : BaseEffect
{
    public int HealthAmount;
    public override void Apply()
    {
        //to-do Вызов увеличения здоровья игрока
        Debug.Log($"Здоровье Персонажа изменено на {HealthAmount}");
    }
}