using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

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
    [Inject] private SignalManager _signalManager;
    //TODO: Раскомментировать эту строку, когда враги на всех сценах будут создаваться через Спаунер
    //private List<ISignal> AudioSignals => _signalManager.PlayerNoiseChanel.GetAllActive();
    private List<ISignal> AudioSignals => SignalManager.Instance.PlayerNoiseChanel.GetAllActive();
    private ISignal _detectedSignal;

    public override Vector3? SignalSourcePosition => _detectedSignal?.SourcePosition;

    protected override float SignalInFieldOfView()
    {
        ISignal maxAudioSignal = null;
        float maxDetectedStrength = 0;
        for (int i = 0; i < AudioSignals.Count; i++)
        {
            ISignal signal = AudioSignals[i];
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

    public override bool SignalInAttackRay(Vector3 targetPosition)
    {
        return false;
    }

    private float CalculateSignalStrength(ISignal signal)
    {
        var distanceToSignal = Vector3.Distance(transform.position, signal.SourcePosition);

        if (distanceToSignal > Distance)
        {
            // Если сигнал за пределами зоны слышимости, то громкость сигнала уменьшается от расстояния
            var effectiveDistance = distanceToSignal - Distance;
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
        Gizmos.DrawWireSphere(transform.position, Distance);
    }
}
