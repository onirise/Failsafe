using UnityEngine;

/// <summary>
/// Базовый интерфейс для сигналов, котрые могут обнаружить противники
/// </summary>
public interface ISignal
{
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
/// Аудио сигнал
/// </summary>
public interface IAudioSignal : ISignal
{
}

/// <summary>
/// Класс аудио сигнала, используемый игроком
/// </summary>
public class PlayerAudioSignal : IAudioSignal
{
    public Vector3 SourcePosition { get; private set; }

    public float SignalStrength { get; private set; }

    public void Update(Vector3 position, float strength)
    {
        SourcePosition = position;
        SignalStrength = strength;
    }
}
