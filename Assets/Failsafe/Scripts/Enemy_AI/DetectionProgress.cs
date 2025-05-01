using UnityEngine;
using UnityEngine.UI;

public class DetectionProgress : MonoBehaviour
{
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
    public  bool _playerInFarZone;
    public bool _playerInNearZone;
    private float _lastUpdateTime;
    private const float UPDATE_INTERVAL = 0.05f; // 20 раз в секунду

    public event System.Action<float> OnProgressChanged;
    public event System.Action OnDetected;
    private float _lastVisualProgress = -1f; // -1, чтобы принудительно обновить слайдер при старте

    private void Awake()
    {
        _currentFastSpeed = _baseFastZoneSpeed;
        _currentSlowSpeed = _baseSlowZoneSpeed;

        if (_detectionSlider == null)
            _detectionSlider = GetComponentInChildren<Slider>();
    }

    public void SetDetectionZones(bool inFarZone, bool inNearZone)
    {
        _playerInFarZone = inFarZone;
        _playerInNearZone = inNearZone;
    }

    private void Update()
    {
        if (Time.time - _lastUpdateTime < UPDATE_INTERVAL)
            return;

        _lastUpdateTime = Time.time;
        UpdateDetection();
    }

    private void UpdateDetection()
    {
        float detectionRate = _playerInNearZone ? _currentFastSpeed :
                              _playerInFarZone ? _currentSlowSpeed :
                              _detectionProgress > 0 ? -_decaySpeed : 0f;

        if (detectionRate != 0f)
        {
            AddDetection(detectionRate * UPDATE_INTERVAL);
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
                OnDetected?.Invoke();
            }
        }
    }


    private void UpdateVisuals()
    {
        if (_detectionSlider != null)
        {
            float normalized = _detectionProgress / 100f;

            // Обновляем только если значение действительно изменилось
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