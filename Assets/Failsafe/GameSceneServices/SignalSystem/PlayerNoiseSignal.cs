using Cysharp.Threading.Tasks;
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

    private float _targetStrength;
    private float _decayPerSecond = 5f;
    private UniTask _decayStrengthTask;

    /// <summary>
    /// Обновить силу издаваемого шума игроком
    /// </summary>
    /// <param name="strength"></param>
    public void UpdateStrength(float strength)
    {
        _targetStrength = strength;
        if (_targetStrength >= SignalStrength)
        {
            SignalStrength = _targetStrength;
        }
        else if (_decayStrengthTask.Status != UniTaskStatus.Pending)
        {
            _decayStrengthTask = DecayStrength();
        }
    }

    private async UniTask DecayStrength()
    {
        while (_targetStrength < SignalStrength)
        {
            SignalStrength -= _decayPerSecond * Time.deltaTime;
            await UniTask.Yield();
        }
        SignalStrength = _targetStrength;
    }

    public override string ToString() =>
        $"[{SourcePosition}] ({SignalStrength}) {nameof(PlayerNoiseSignal)}";
}
