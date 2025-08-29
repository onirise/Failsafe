using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy_ScriptableObject : ScriptableObject
{
    [Header("Enemy Parameters")]
    public string enemyName; // Имя врага
    public float accelaration = 120; // Ускорение врага, как быстро он может развить максимальную скорость измерения метры в секунду
    public int angelarSpeed = 120; // Угол поворота врага, как быстро он может повернуться измерения градус в секунду

    [Header("Сенсоры")] 
    public float OriginDistanceSight = 20f; //Изначальное расстояние обзора противника
    public float AlertDistanceSight = 30f; //При тревоге
    public float OrginAngleViev = 45f; //Изначальный угол обзора
    public float AlertAngleViev = 65f; //При тревоге
    public float VisualFocusTime = 1f;
    public float AlertVisualFocusTime = 0.2f;
    
    public float OriginDistanceHearing = 10f; //Изначальный радиус слуха
    public float AlertDistanceHearing = 20f; //При тревоге
    public float MinSoundStr = 0.5f;
    public float MaxSoundStr = 10f;
    public float AlertMinSoundStr = 0.1f;
    public float HearFocusTime = 1f;
    public float AlertHearFocusTime = 0.2f;
    
    

    [Header("Awareness meter")] 
    public float FillSpeed = 80f; //Множитель заполнения шкалы от силы сигнала за единицу времени
    public float DecaySpeed = 10f; //Множитель скорсоти снижения настороженности за единицу времени
    public float DecayDelay = 2f; //Задержка перед началом сниения настороженности
    
    public float AlertThreshold = 30f; // Нижняя граница снижения настороженности после преследования
    public float ChaseThreshold = 100f; // Минимальная граница начала преследования
    public float ChaseExitThreshold = 30f; //Минимальная гринца прекращения преследования
    
    [Header("Check")]
    public float CheckRadius;
    public float CheckDuration;
    public float CheckInterval;
    
    [Header("Chase")]
    public float ChaseSpeed = 6f; // Скорость преследования
    
    [Header("Patrolling")]
    public float PatrolingSpeed = 4f; // Скорость патрулирования
    public float PatrollingWaitTime = 2f; // Время ожидания при патрулировании
    
    [Header("Enemy Searching")]
    public float SearchingSpeed = 3f; // Скорость поиска
    public float SearchingDuration = 2f; // Время ожидания при поиске
    public float SearchRadius = 5f; // Радиус поиска
    public float offsetSearchingPoint = 10f; // Радиус области поиска
    public float changePointInterval = 1f;
    
    [Header("Health")]
    public float enemyHealth = 100f; // Здоровье врага

    [Header("Stun")]
    public int MinStunTime; //Если высчитаный скриптом результат меньше этого значения - враг не станится
    public int MaxStunTime; //Верхняя граница стана, результаты выше понижаются до этого параметра
    public float StunMultiplier; //Множитель стана, чтобы подогнать результат произведения массы на скорость к желаемому значению времени стана

    [Header("Attack")]
    public float Damage = 100f; // Урон врага
    public float AttackRangeMin = 10f; // Минимальная дальность атаки врага
    public float AttackRangeMax = 15f; // Максимальная дальность атаки врага
    public float AttackDuration = 5f; // Длительность атаки
    public float AttackDelay = 1f; // задержка перед атакой
    public float AttackCooldown = 4f; // Скорость атаки врага


}
