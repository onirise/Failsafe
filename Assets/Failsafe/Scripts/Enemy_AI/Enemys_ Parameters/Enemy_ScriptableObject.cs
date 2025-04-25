using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy_ScriptableObject : ScriptableObject
{
    [Header("Enemy Parameters")]
    public string enemyName; // Имя врага
    public float accelaration = 120; // Ускорение врага, как быстро он может развить максимальную скорость измерения метры в секунду
    public int angelarSpeed = 120; // Угол поворота врага, как быстро он может повернуться измерения градус в секунду
    public float stoppingDistance = 0.5f; // Дистанция остановки врага

    [Header("Enemy Chase")]
    public float enemyChaseSpeed = 6f; // Скорость преследования
    public float enemyLostPlayerTime = 5f; // Время потери игрока
    [Header("Enemy Patroling")]
    public float enemyPatrolingSpeed = 4f; // Скорость патрулирования
    public float enemyPatrolingWaitTime = 2f; // Время ожидания при патрулировании
    [Header("Enemy Searching")]
    public float enemySearchingSpeed = 3f; // Скорость поиска
    public float enemySearchingDuration = 2f; // Время ожидания при поиске
    public float enemySearchRadius = 5f; // Радиус поиска
    public float offsetSearchingPoint = 10f; // Радиус области поиска
    [Header("Enemy Health")]
    public float enemyHealth = 100f; // Здоровье врага
    [Header("Enemy Damage")]
    public float enemyDamage = 10f; // Урон врага
    [Header("Enemy Attack Range")]
    public bool isRangeAttack = true; // Флаг, указывающий, является ли атака ближнего боя
    public float enemyAttackRange = 2f; // Дальность атаки врага
    [Header("Enemy Attack Speed")]
    public float enemyAttackSpeed = 1f; // Скорость атаки врага


}
