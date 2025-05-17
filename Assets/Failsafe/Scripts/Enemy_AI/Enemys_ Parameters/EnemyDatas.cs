using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatas", menuName = "Data/EnemyDatas", order = 1)]
public class EnemyDatas : ScriptableObject
{
    public EnemyData[] enemyDatas; // Массив данных врагов
}

[System.Serializable]
public class EnemyData
{
    public string Name; // Имя врага
    public float Accelaration; // Ускорение врага, как быстро он может развить максимальную скорость измерения метры в секунду
    public int AngelarSpeed ; // Угол поворота врага, как быстро он может повернуться измерения градус в секунду
    public float StoppingDistance; // Дистанция остановки врага
    [Header("Enemy Chase")]
    public float ChaseSpeed; // Скорость преследования
    public float LostPlayerTime; // Время потери игрока
    [Header("Enemy Patroling")]
    public float PatrolingSpeed; // Скорость патрулирования
    public float PatrolingWaitTime; // Время ожидания при патрулировании
    [Header("Enemy Searching")]
    public float SearchingSpeed; // Скорость поиска
    public float SearchingDuration; // Время ожидания при поиске
    public float SearchRadius; // Радиус поиска
    public float offsetSearchingPoint; // Радиус области поиска
    [Header("Enemy Health")]
    public float Health; // Здоровье врага
    [Header("Enemy Damage")]
    public float Damage; // Урон врага
    [Header("Enemy Attack Range")]
    public bool isRangeAttack = true; // Флаг, указывающий, является ли атака ближнего боя
    public float AttackRange; // Дальность атаки врага
    [Header("Enemy Attack Speed")]
    public float AttackSpeed; // Скорость атаки врага
}
