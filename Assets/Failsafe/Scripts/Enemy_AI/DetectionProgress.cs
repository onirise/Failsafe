using System;
using UnityEngine;
using UnityEngine.UI;

public class DetectionProgress : MonoBehaviour
{
    [Header("Скорости обнаружения")]
    [SerializeField] private float _fastZoneDetectionSpeed = 20f; // % в секунду
    [SerializeField] private float _slowZoneDetectionSpeed = 5f;

    [Header("Текущий статус")]
    [Range(0, 100)]
    [SerializeField] private float _detectionProgress;

    public event Action<float> OnProgressChanged;
    public event Action OnDetected;

    public bool plyerInFarZone, playerInNearZone;

    public float[] modifiers;

    [SerializeField] private Slider slider;
    private void Start()
    {
        _detectionProgress = 0;
    }

    private void Update()
    {
        if (plyerInFarZone)
        {
            AddDetection(_slowZoneDetectionSpeed * Time.deltaTime);
        }
        else if (playerInNearZone)
        {
            AddDetection(_fastZoneDetectionSpeed * Time.deltaTime);
        }
        else
        {
            if(_detectionProgress > 0) AddDetection(-_slowZoneDetectionSpeed * Time.deltaTime);

        }
        SliderFillUp();
    }

    private void SliderFillUp()
    {
        if (slider != null)
        {
            slider.value = _detectionProgress / 100;
        }
        else
        {
            Debug.LogError("Slider is not assigned in the inspector.");
        }
    }
    private void AddDetection(float amount)
    {
        float newProgress = Mathf.Clamp(_detectionProgress + amount, 0, 100);

        if (Mathf.Abs(newProgress - _detectionProgress) > 0.1f)
        {
            _detectionProgress = newProgress;
            OnProgressChanged?.Invoke(_detectionProgress);
            Debug.Log($"Detection Progress: {_detectionProgress}");

            if (_detectionProgress >= 100)
            {
                OnDetected?.Invoke();
            }
        }
    }

    

}
