using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DetectionProgress : MonoBehaviour
{
    public enum DetectionZone
    {
        None,
        Far,
        Near
    }
    public DetectionZone CurrentZone = DetectionZone.None;
    [Header("Detection Settings")]
    [SerializeField] private int _baseFastZoneSpeed = 20;
    [SerializeField] private int _baseSlowZoneSpeed = 5;
    [SerializeField] private float _decaySpeed = 15f;
    [SerializeField] private Slider _detectionSlider;

    [Header("Debug")]
    [Range(0, 100)]
    [SerializeField] private float _detectionProgress;
    private float _currentFastSpeed;
    private float _currentSlowSpeed;
    private float _lastUpdateTime;
    private const float _uPDATE_INTERVAL = 0.05f;
    private float _lastVisualProgress = -1f;
    public event System.Action<float> OnProgressChanged;
    public event System.Action OnDetected;
    public bool InChase = false; // Новый флаг

    private void Awake()
    {
        _detectionSlider.value = 0f;
        _currentFastSpeed = _baseFastZoneSpeed;
        _currentSlowSpeed = _baseSlowZoneSpeed;

        if (_detectionSlider == null)
            _detectionSlider = GetComponentInChildren<Slider>();
    }


    private void Update()
    {
        if (Time.time - _lastUpdateTime < _uPDATE_INTERVAL)
            return;

        _lastUpdateTime = Time.time;
        UpdateDetection();
    }

    private void UpdateDetection()
    {
        float detectionRate = 0f;

        switch (CurrentZone)
        {
            case DetectionZone.Near:
                detectionRate = _currentFastSpeed;
                break;
            case DetectionZone.Far:
                detectionRate = _currentSlowSpeed;
                break;
            case DetectionZone.None:
                detectionRate = _detectionProgress > 0 ? -_decaySpeed : 0f;
                if(_detectionProgress <= 0f)
                {
                    InChase = false; // Сбрасываем флаг, если прогресс обнаружения обнулен
                    _detectionProgress = 0f; // Обнуляем прогресс, чтобы избежать отрицательных значений
                }
                break;
        }

        if (detectionRate != 0f)
        {
            AddDetection(detectionRate * _uPDATE_INTERVAL);
        }
       
    }

    private void AddDetection(float amount)
    {
        float newProgress = Mathf.Clamp(_detectionProgress + amount, 0f, 100f);

        if (Mathf.Abs(newProgress - _detectionProgress) > 0.1f)
        {
            _detectionProgress = newProgress;
            UpdateVisuals();
            OnProgressChanged?.Invoke(_detectionProgress);

            if (_detectionProgress >= 100f)
            {
                InChase = true; // Устанавливаем флаг, когда обнаружение завершено
                OnDetected?.Invoke();
            }
        }
    }

    private void UpdateVisuals()
    {
        if (_detectionSlider != null)
        {
            float normalized = _detectionProgress / 100f;

            if (!Mathf.Approximately(normalized, _lastVisualProgress))
            {
                _detectionSlider.value = normalized;
                _lastVisualProgress = normalized;
            }
        }
    }

    public void ModifyMultiplier(float multiplier)
    {
        if (multiplier <= 0)
        {
            Debug.LogError("Multiplier must be positive");
            return;
        }

        _currentFastSpeed = _baseFastZoneSpeed * multiplier;
        _currentSlowSpeed = _baseSlowZoneSpeed * multiplier;
        Debug.Log($"Detection speed modified: Fast = {_currentFastSpeed}, Slow = {_currentSlowSpeed}");
    }
}