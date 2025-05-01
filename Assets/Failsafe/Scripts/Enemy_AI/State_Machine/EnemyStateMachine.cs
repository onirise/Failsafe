using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
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

    // private EnemyStateFactory _stateFactory;
    private EnemyBaseState _currentState; // Приватное поле
    public EnemyBaseState CurrentState => _currentState; // Свойство для доступа к текущему состоянию
    [SerializeField] private string currentStateName; // Для отладки

    void Awake()
    {
        FOV = GetComponent<FieldOfView>();
        Agent = GetComponent<NavMeshAgent>();
        OnDetectPlayer();
    }

    void Start()
    {
        SwitchState(EnemyStateType.Patrol);
    }

    void Update()
    {
        CurrentState?.UpdateState(this); // Используем свойство
        currentStateName = CurrentState?.GetType().Name; // Отображаем имя состояния
    }

    public void SwitchState(EnemyStateType newState)
    {
        _currentState?.ExitState(this); // Уходим из предыдущего состояния
        _currentState = EnemyStateFactory.CreateState(newState);
        _currentState.EnterState(this);
    }

    private void OnDetectPlayer()
    {
        player.GetComponent<DetectionProgress>().OnDetected += () => SwitchState(EnemyStateType.Chase);
    }

}


