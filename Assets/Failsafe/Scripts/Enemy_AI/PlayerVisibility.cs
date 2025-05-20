using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    [Header("Visibility Settings")]
    [Range(0, 100)]
    [SerializeField] private float _playerVisScore;
    [SerializeField] private DetectionProgress _detectionProgress;
    [SerializeField] private string displayStatus;

    [Header("Modifiers")]
    [SerializeField] private float _lowModifier = 0.5f;
    [SerializeField] private float _mediumModifier = 1f;
    [SerializeField] private float _highModifier = 2f;

    public enum VisibilityStatus { Low, Medium, High }
    private VisibilityStatus _currentStatus;

    // Свойство с отслеживанием изменений
    public float PlayerVisScore
    {
        get => _playerVisScore;
        set
        {
            if (Mathf.Abs(_playerVisScore - value) > 0.1f) // Порог изменения
            {
                _playerVisScore = Mathf.Clamp(value, 0, 100);
                UpdateStatus();
            }
        }
    }

    private void Awake()
    {
        // Инициализация при старте
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        var newStatus = GetStatusForScore(_playerVisScore);

        if (_currentStatus != newStatus)
        {
            _currentStatus = newStatus;
            displayStatus = _currentStatus.ToString();
            ApplyModifier(_currentStatus);
            Debug.Log($"Status changed to {_currentStatus}");
        }
    }

    private VisibilityStatus GetStatusForScore(float score)
    {
        if (score < 30f) return VisibilityStatus.Low;
        if (score < 70f) return VisibilityStatus.Medium;
        return VisibilityStatus.High;
    }

    private void ApplyModifier(VisibilityStatus status)
    {
        switch (status)
        {
            case VisibilityStatus.Low:
                _detectionProgress.ModifyMultiplier(_lowModifier);
                break;
            case VisibilityStatus.Medium:
                _detectionProgress.ModifyMultiplier(_mediumModifier);
                break;
            case VisibilityStatus.High:
                _detectionProgress.ModifyMultiplier(_highModifier);
                break;
        }
    }

    // Для отладки в инспекторе
    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        UpdateStatus();
    }

    public void ResetVisibility()
    {
        PlayerVisScore = 0;
    }
}
