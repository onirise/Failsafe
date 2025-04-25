using System;
using UnityEngine;

/// <summary>
/// Сенсор, который использую враги для обнаружения игрока
/// </summary>
public abstract class Sensor : MonoBehaviour
{
    private float _signalStrength;

    /// <summary>
    /// Время на фокусировку сенсора <para/>
    /// Используется для симуляции процесса распознования сигнала, 
    /// например апартная задерка между камерой и мозгом врага
    /// </summary>
    [SerializeField]
    protected float _focusingTime;
    [SerializeField]
    protected float _focusingProgress;

    /// <summary>
    /// Значение сигнала попавшего в зону видимости сенсора от 0 до 1.
    /// </summary>
    /// <returns>0 если не попал в зону видимости, если попал то сила полученого сигнала от 0 до 1</returns>
    protected abstract float SignalInFieldOfView();

    /// <summary>
    /// Сенсор активирован
    /// </summary>
    /// <returns></returns>
    public bool IsActivated() => SignalStrength > 0;

    /// <summary>
    /// Сила обнаруженного сингала от 0 до 1
    /// </summary>
    public float SignalStrength => _focusingProgress >= _focusingTime ? _signalStrength : 0;

    /// <summary>
    /// Координаты источник сигнала
    /// </summary>
    public abstract Vector3? SignalSourcePosition { get; }

    protected virtual void Update()
    {
        _signalStrength = Math.Clamp(SignalInFieldOfView(), 0, 1);
        if (_signalStrength > 0)
        {
            if (_focusingProgress < _focusingTime)
            {
                _focusingProgress += Time.deltaTime;
            }
        }
        else
        {
            if (_focusingProgress >= 0)
            {
                _focusingProgress -= Time.deltaTime;
            }
        }
    }
}
