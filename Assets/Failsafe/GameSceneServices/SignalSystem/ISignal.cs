using System;
using UnityEngine;

/// <summary>
/// Тип сигнала
/// </summary>
public enum SignalType
{
    /// <summary>
    /// Шум
    /// </summary>
    Noise,
    /// <summary>
    /// Электромагнитное излучение
    /// </summary>
    EMR,
    /// <summary>
    /// Видимый спектр
    /// </summary>
    Visual
}

/// <summary>
/// Базовый интерфейс для сигналов, котрые могут обнаружить противники
/// </summary>
public interface ISignal
{
    /// <summary>
    /// Тип сигнала
    /// </summary>
    public SignalType Type { get; }
    /// <summary>
    /// Позиция источника сигнала
    /// </summary>
    public Vector3 SourcePosition { get; }
    /// <summary>
    /// Сила источника сигнала
    /// </summary>
    public float SignalStrength { get; }
}

/// <summary>
/// Сигнал с ограниченным временем жизни
/// </summary>
public interface ITemporarySignal : ISignal
{
    /// <summary>
    /// Время когда сигнал испарится
    /// </summary>
    public float ExpireAt { get; }

    /// <summary>
    /// Уменьшить силу сигнала со временем
    /// </summary>
    /// <param name="deltaTime">Время с последнего обновления</param>
    public void Decay(float deltaTime);

    /// <summary>
    /// </summary>
    /// <param name="duration">Время жизни сигнала</param>
    public void Initialize(Vector3 position, float strength, float duration = 1);

    /// <summary>
    /// Метод выполняемый в момент испарения сигнала
    /// </summary>
    public void OnExpier();
}
