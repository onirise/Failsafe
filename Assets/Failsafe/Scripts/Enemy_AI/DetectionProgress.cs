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
    public DetectionZone currentZone = DetectionZone.None;
    [Header("Detection Settings")]
    [SerializeField] private int baseFastZoneSpeed = 20;
    [SerializeField] private int baseSlowZoneSpeed = 5;
    [SerializeField] private float decaySpeed = 15f;
    [SerializeField] private Slider detectionSlider;

    [Header("Debug")]
    [Range(0, 100)]
    [SerializeField] private float detectionProgress;
    private float currentFastSpeed;
    private float currentSlowSpeed;
    private float lastUpdateTime;
    private const float UPDATE_INTERVAL = 0.05f;
    private float lastVisualProgress = -1f;
    public event System.Action<float> OnProgressChanged;
    public event System.Action OnDetected;
    public bool inChase = false; // Новый флаг

    private void Awake()
    {
        detectionSlider.value = 0f;
        currentFastSpeed = baseFastZoneSpeed;
        currentSlowSpeed = baseSlowZoneSpeed;

        if (detectionSlider == null)
            detectionSlider = GetComponentInChildren<Slider>();
    }


    private void Update()
    {
        if (Time.time - lastUpdateTime < UPDATE_INTERVAL)
            return;

        lastUpdateTime = Time.time;
        UpdateDetection();
    }

    private void UpdateDetection()
    {
        float detectionRate = 0f;

        switch (currentZone)
        {
            case DetectionZone.Near:
                detectionRate = currentFastSpeed;
                break;
            case DetectionZone.Far:
                detectionRate = currentSlowSpeed;
                break;
            case DetectionZone.None:
                detectionRate = detectionProgress > 0 ? -decaySpeed : 0f;
                if(detectionProgress <= 0f)
                {
                    inChase = false; // Сбрасываем флаг, если прогресс обнаружения обнулен
                    detectionProgress = 0f; // Обнуляем прогресс, чтобы избежать отрицательных значений
                }
                break;
        }

        if (detectionRate != 0f)
        {
            AddDetection(detectionRate * UPDATE_INTERVAL);
        }
       
    }

    private void AddDetection(float amount)
    {
        float newProgress = Mathf.Clamp(detectionProgress + amount, 0f, 100f);

        if (Mathf.Abs(newProgress - detectionProgress) > 0.1f)
        {
            detectionProgress = newProgress;
            UpdateVisuals();
            OnProgressChanged?.Invoke(detectionProgress);

            if (detectionProgress >= 100f)
            {
                inChase = true; // Устанавливаем флаг, когда обнаружение завершено
                OnDetected?.Invoke();
            }
        }
    }

    private void UpdateVisuals()
    {
        if (detectionSlider != null)
        {
            float normalized = detectionProgress / 100f;

            if (!Mathf.Approximately(normalized, lastVisualProgress))
            {
                detectionSlider.value = normalized;
                lastVisualProgress = normalized;
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

        currentFastSpeed = baseFastZoneSpeed * multiplier;
        currentSlowSpeed = baseSlowZoneSpeed * multiplier;
        Debug.Log($"Detection speed modified: Fast = {currentFastSpeed}, Slow = {currentSlowSpeed}");
    }
}