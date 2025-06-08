﻿using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Стандартное поведение противника пока он не обнаружил игрока
/// </summary>
public class DefaultState : BehaviorState
{
    private Sensor[] _sensors;
    private Transform _transform;
    EnemyController _enemyController;

    public bool IsPatroling()
    {
        return true;
    }
    public DefaultState(Sensor[] sensors, Transform transform, EnemyController enemyController)
    {
        _sensors = sensors;
        _transform = transform;
        _enemyController = enemyController;
    }

    private float _warningProgress;
    private float _warningTime = 1;


    public bool PlayerSpotted() => _warningProgress >= _warningTime;

    public override void Enter()
    {
        base.Enter();
        _warningProgress = 0;
        Debug.Log("Enter DefaultState");
    }

    public override void Update()
    {
        foreach (var sensor in _sensors)
        {

            if (sensor.IsActivated())
            {
                _warningProgress += sensor.SignalStrength * Time.deltaTime;
            }
        }
    }
}