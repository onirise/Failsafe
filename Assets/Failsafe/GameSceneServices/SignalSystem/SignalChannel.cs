using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Система отслеживания всех сигналов опрделенного типа
/// </summary>
public class SignalChannel
{
    private ObjectPool<ITemporarySignal> _signalPool;
    private List<ISignal> _activeSignals;

    public SignalChannel(Func<ITemporarySignal> createSignal, int maxPoolSize = 1000)
    {
        _activeSignals = new List<ISignal>(maxPoolSize);
        _signalPool = new ObjectPool<ITemporarySignal>(
            createSignal,
            maxSize: maxPoolSize
        );
    }

    /// <summary>
    /// Добавить постоянный сигнал
    /// </summary>
    /// <param name="signal"></param>
    public void AddConstant(ISignal signal)
    {
        _activeSignals.Add(signal);
    }

    /// <summary>
    /// Добавить временный сигнал
    /// </summary>
    /// <param name="signal"></param>
    /// <param name="duration">Время жизни сигнала</param>
    public void Add(Vector3 position, float strength, float duration)
    {
        var signalFromPool = _signalPool.Get();
        signalFromPool.Initialize(position, strength, duration);
        _activeSignals.Add(signalFromPool);
    }

    /// <summary>
    /// Уменьшить силу сигналов
    /// </summary>
    /// <param name="deltaTime">Время с последнего вызова</param>
    public void DecaySignals(float deltaTime)
    {
        for (int i = _activeSignals.Count - 1; i >= 0; i--)
        {
            var tempSignal = _activeSignals[i] as ITemporarySignal;
            if (tempSignal == null) continue;

            tempSignal.Decay(deltaTime);
        }
    }

    /// <summary>
    /// Удалить из канала просроченные сигналы
    /// </summary>
    /// <param name="currentTick">Время когда запущена операция</param>
    public void RemoveExpiredSignals(float currentTick)
    {
        for (int i = _activeSignals.Count - 1; i >= 0; i--)
        {
            var tempSignal = _activeSignals[i] as ITemporarySignal;
            if (tempSignal == null) continue;
            if (tempSignal.ExpireAt > currentTick) continue;

            tempSignal.OnExpier();
            _activeSignals.RemoveAt(i);
            _signalPool.Release(tempSignal);
        }
    }

    /// <summary>
    /// Получить список всех активных сигналов на уровне
    /// </summary>
    /// <returns></returns>
    public List<ISignal> GetAllActive()
    {
        return _activeSignals;
    }
}
