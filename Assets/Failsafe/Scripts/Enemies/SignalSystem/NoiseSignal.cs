using System;
using UnityEngine;

/// <summary>
/// Реализация постоянного шума
/// </summary>
public class NoiseSignal : ISignal
{
    public SignalType Type => SignalType.Noise;

    public Vector3 SourcePosition { get; protected set; }

    public float SignalStrength { get; protected set; }


    public NoiseSignal(Vector3 sourcePosition, float signalStrength)
    {
        SourcePosition = sourcePosition;
        SignalStrength = signalStrength;
    }

    public static NoiseSignal Zero => new NoiseSignal(Vector3.zero, 0);

    public override string ToString() =>
        $"[{SourcePosition}] ({SignalStrength}) {nameof(ISignal)}";
}

/// <summary>
/// Реализация временного шума
/// </summary>
public class TempNoiseSignal : NoiseSignal, ITemporarySignal
{
    private float _createtAt;
    public float ExpireAt { get; private set; }

    public TempNoiseSignal(Vector3 sourcePosition, float signalStrength, float duration = 5) : base(sourcePosition, signalStrength)
    {
        _createtAt = Time.time;
        ExpireAt = _createtAt + duration;
    }

    public void Initialize(Vector3 position, float strength, float duration = 5)
    {
        _createtAt = Time.time;
        SourcePosition = position;
        SignalStrength = strength;
        ExpireAt = _createtAt + duration;
    }

    public void OnExpier()
    {
    }

    public static new TempNoiseSignal Zero => new TempNoiseSignal(Vector3.zero, 0, 0);

    public override string ToString() =>
        $"[{SourcePosition}] ({SignalStrength}) {nameof(ITemporarySignal)} Created at {_createtAt}; Expire at {ExpireAt}";
}


/// <summary>
/// Реализация сигнала на GameObject с колайдером в виде сферы
/// ХЗ зачем, может удалить
/// </summary>
[RequireComponent(typeof(Collider))]
public class NoiseSignalObject : MonoBehaviour, ITemporarySignal
{
    public SignalType Type => SignalType.Noise;
    public Vector3 SourcePosition => transform.position;

    public float SignalStrength => _collider.radius;

    public float ExpireAt { get; private set; }

    private SphereCollider _collider;


    public void Initialize(Vector3 position, float strength, float duration = 1)
    {
        transform.position = position;
        _collider.radius = strength;
        ExpireAt = Time.time + duration;
    }

    public void OnExpier()
    {
    }
}
