using System;
using UnityEngine;

/// <summary>
/// Реализация обнаружения игрока на слух
/// </summary>
public class AudioSensor : Sensor
{
    [SerializeField]
    private float _minSoundStrength = 20;
    /// <summary>
    /// Уровень шума, для которого устанавливается максимальная сила сигнала
    /// </summary>
    [SerializeField]
    private float _maxSoundStrength = 50;
    [SerializeField]
    private float _effectiveSoundDistance = 20;
    /// <summary>
    /// Объект, который нужно обнаружить, в данном случае игрок
    /// </summary>
    public DemoPlayer _target;
    // пока проверяем только игрока, 
    // позже нужна система регистрации всех звуков на уровне (падающих предметов, отвлекающих гаджетов)
    private IAudioSignal _targetAudioSignal => _target.Noise;

    public override Vector3? SignalSourcePosition => _targetAudioSignal.SourcePosition;

    protected override float SignalInFieldOfView()
    {
        if (_targetAudioSignal.SignalStrength < _minSoundStrength) return 0;

        var distance = Vector3.Distance(transform.position, _targetAudioSignal.SourcePosition);

        // TODO: поменять формулу угасания шума от расстояния
        // сейчас значение шума сильно уменьшается на расстоянии
        var effectiveDistance = Math.Max(distance - _effectiveSoundDistance, 1);
        var detectedSoundStrength = _targetAudioSignal.SignalStrength / effectiveDistance;

        if (detectedSoundStrength < _minSoundStrength) return 0;

        return Math.Clamp(detectedSoundStrength / _maxSoundStrength, 0, 1);
    }
}