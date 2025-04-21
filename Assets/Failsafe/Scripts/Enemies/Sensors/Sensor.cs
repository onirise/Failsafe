using UnityEngine;

/// <summary>
/// Сенсор, который использую враги для обнаружения игрока
/// </summary>
public abstract class Sensor : MonoBehaviour
{
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
    /// Возвращает объект, который активировал сенсор
    /// </summary>
    public abstract Transform ObjectThatActivatedSensor { get; }

    /// <summary>
    /// Объект попал в облость сенсора
    /// </summary>
    /// <returns></returns>
    public abstract bool IsInFieldOfView();

    /// <summary>
    /// Сенсор активирован
    /// </summary>
    /// <returns></returns>
    public bool IsActivated() => _focusingProgress >= _focusingTime;

    void Update()
    {
        if (IsInFieldOfView())
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
