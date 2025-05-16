using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Реализация обнаружения игрока на слух
/// </summary>
public class NoiseSensor : Sensor
{
    [SerializeField]
    private float _minSoundStrength = 1;
    /// <summary>
    /// Уровень шума, для которого устанавливается максимальная сила сигнала
    /// </summary>
    [SerializeField]
    private float _maxSoundStrength = 10;
    private List<ISignal> _audioSignals => SignalManager.Instance.PlayerNoiseChanel.GetAllActive();
    private ISignal _detectedSignal;

    public override Vector3? SignalSourcePosition => _detectedSignal?.SourcePosition;

    protected override float SignalInFieldOfView()
    {
        ISignal maxAudioSignal = null;
        float maxDetectedStrength = 0;
        for (int i = 0; i < _audioSignals.Count; i++)
        {
            ISignal signal = _audioSignals[i];
            if (signal.SignalStrength < _minSoundStrength) continue;

            float detectedSoundStrength = CalculateSignalStrength(signal);
            if (detectedSoundStrength < _minSoundStrength) continue;

            if (detectedSoundStrength > maxDetectedStrength)
            {
                maxAudioSignal = signal;
                maxDetectedStrength = detectedSoundStrength;
            }
        }
        _detectedSignal = maxAudioSignal;
        return Math.Clamp(maxDetectedStrength / _maxSoundStrength, 0, 1);
    }

    private float CalculateSignalStrength(ISignal signal)
    {
        var distanceToSignal = Vector3.Distance(transform.position, signal.SourcePosition);

        if (distanceToSignal > _distance)
        {
            // Если сигнал за пределами зоны слышимости, то громкость сигнала уменьшается от расстояния
            var effectiveDistance = distanceToSignal - _distance;
            var detectedSoundStrength = signal.SignalStrength / effectiveDistance;
            return detectedSoundStrength;
        }
        // Если сигнал в пределах зоны слышимости то обычная сила сигнала
        // Возможно нужно поменять формулу, чтобы сигналы ближе к сенсору казались громче
        return signal.SignalStrength;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distance);
    }
}
