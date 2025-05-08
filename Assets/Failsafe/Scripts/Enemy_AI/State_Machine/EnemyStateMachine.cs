using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyStateConfig stateConfig;  // Ссылка на ScriptableObject
    private Dictionary<string, Type> _stateLookup = new();  // Словарь для поиска типов по имени состояния
    [Header("References")]
    public GameObject player;
    public GameObject enemyWeapon;
    public FieldOfView FOV { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    [Header("State Settings")]
    public float normalSpeed = 3f;
    public float patrolSpeed = 3f;
    public float waitTime = 2f;
    public GameObject[] patrolPoints;
    public float lostPlayerTimer = 5f;
    public float enemyChaseSpeed = 8f;
    public bool afterChase = false; // Флаг, указывающий, что враг был в состоянии погони
    [Header("Search Settings")]
    public float searchRadius = 10f;
    public float searchDuration = 5f;
    public float timeToGet = 5f; // Время, за которое враг должен добраться до точки поиска
    public float changePointTimer = 5f; // Таймер для смены точки поиска
    public float offsetSearchinPoint = 5f; // Таймер для смены точки поиска
    public Vector3 searchingPoint; // Точка поиска
    private EnemyBaseState _currentState; // Приватное поле для хранения текущего состояния
    public EnemyBaseState CurrentState => _currentState; // Свойство для доступа к текущему состоянию
    [SerializeField] private string currentStateName; // Для отладки
    public int EnemyID { get; private set; }
    private DetectionProgress playerDetection;

    void Awake()
    {
        EnemyID = GetInstanceID();
        FOV = GetComponent<FieldOfView>();
        Agent = GetComponent<NavMeshAgent>();

        if (Agent == null)
        {
            Debug.LogError($"NavMeshAgent не найден на объекте: {gameObject.name}");
        }

        // Инициализация состояний
        InitializeStates();
    }

    void Start()
    {
        playerDetection = player.GetComponent<DetectionProgress>();
        InitializeStates();
        SetupPlayerDetection();
        SwitchState<EnemyPatrolingState>();
    }

    void Update()
    {
        CurrentState?.UpdateState(this);
        currentStateName = CurrentState?.GetType().Name;
    }

    /// <summary>
    /// Инициализация доступных состояний из ScriptableObject
    /// </summary>
    private void InitializeStates()
    {
        List<Type> enemyStates = new List<Type>();

        foreach (var stateName in stateConfig.AvailableStates)
        {
            Type type = Type.GetType(stateName);

            if (type != null && type.IsSubclassOf(typeof(EnemyBaseState)))
            {
                enemyStates.Add(type);
            }
            else
            {
                Debug.LogWarning($"Состояние {stateName} не найдено или не является наследником EnemyBaseState.");
            }
        }

        // Регистрация этих состояний для конкретного врага
        EnemyStateFactory.RegisterEnemyStates(EnemyID, enemyStates);
    }

    /// <summary>
    /// Переключение состояния
    /// </summary>
    public void SwitchState<T>() where T : EnemyBaseState, new()
    {
        // Завершаем текущее состояние, если оно существует
        if (_currentState != null)
        {
            _currentState.ExitState(this);

            // Проверяем перед возвратом в пул
            EnemyStateFactory.ReturnState(_currentState);
        }

        // Забираем новое состояние из фабрики
        _currentState = EnemyStateFactory.GetState(typeof(T).Name, EnemyID);

        // Если по какой-то причине состояние вернулось null, кидаем ошибку с пояснением
        if (_currentState == null)
        {
            Debug.LogError($"[EnemyStateMachine] Ошибка при переключении состояния: {typeof(T).Name} не было получено из фабрики!");
            return;
        }

        _currentState.EnterState(this);
    }

    private void SetupPlayerDetection()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set in EnemyStateMachine");
            return;
        }

        playerDetection = player.GetComponent<DetectionProgress>();
        if (playerDetection == null)
        {
            Debug.LogError("DetectionProgress not found on player");
            return;
        }
        playerDetection.OnDetected += HandlePlayerDetected;

    }

    private void HandlePlayerDetected()
    {
        // Проверяем, видим ли мы игрока сейчас
        if (FOV.canSeePlayerFar || FOV.canSeePlayerNear)
        {
            SwitchState<EnemyOnDetectPlayerState>();
        }
    }
}

