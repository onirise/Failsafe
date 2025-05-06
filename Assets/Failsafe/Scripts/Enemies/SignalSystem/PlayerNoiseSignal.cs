using System;
using UnityEngine;

/// <summary>
/// Класс аудио сигнала, используемый игроком
/// </summary>
public class PlayerNoiseSignal : ISignal
{
    private readonly Transform _playerTransform;

    public PlayerNoiseSignal(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public SignalType Type => SignalType.Noise;
    public Vector3 SourcePosition => _playerTransform.position;

    public float SignalStrength { get; private set; }

    /// <summary>
    /// Обновить силу издаваемого шума игроком
    /// </summary>
    /// <param name="strength"></param>
    public void UpdateStrength(float strength)
    {
        SignalStrength = strength;
    }

    public override string ToString() =>
        $"[{SourcePosition}] ({SignalStrength}) {nameof(PlayerNoiseSignal)}";
}
